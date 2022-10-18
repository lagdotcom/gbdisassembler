using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lag.GBLib
{
    public class Gameboy : IProject
    {
        public const string ProjectMarker = "GBDPROJ@LR35902:";

        private static readonly Segment[] ramSegments =
        {
            new Segment("WRAM", 0xC000, 0x2000, 0),
            new Segment("HRAM", 0xFF80, 0x79, 0),
        };

        public Gameboy()
        {
            Labeller = new Labeller(this);
            Namer = new Namer(this);
            CPU = new LR35902(this);
            Ports = new List<IPortHandler>();
            Comments = new Dictionary<uint, string>();
            CustomOperands = new Dictionary<uint, Word>();
            Instructions = new Dictionary<uint, IInstruction>();
            Contexts = new Dictionary<uint, InstructionContext>();

            SetupPorts();
        }

        public Gameboy(string filename) : this()
        {
            Filename = filename;
            AcquireROM();
        }

        public string Filename { get; set; }
        public LR35902 CPU;
        public RomHeader Header { get; private set; }
        public List<IPortHandler> Ports { get; private set; }
        public byte[] ROM { get; private set; }
        public IMBC MBC;

        public Dictionary<uint, string> Comments { get; private set; }
        public Dictionary<uint, Word> CustomOperands { get; private set; }
        public Dictionary<uint, IInstruction> Instructions { get; private set; }
        public Dictionary<uint, InstructionContext> Contexts { get; private set; }
        public Labeller Labeller { get; private set; }
        public string Marker => ProjectMarker;
        public Namer Namer { get; private set; }
        public IEnumerable<Segment> Segments => MBC.Segments(Header);
        public IEnumerable<Segment> RAMSegments => ramSegments;

        public uint ROMSize => Header.ROM;

        public int MaxOpSize => 3;

        public IInstruction Parse(uint address) => CPU.Decode(address);

        public void StandardAnalysis()
        {
            Analyse(0x00);
            Analyse(0x08);
            Analyse(0x10);
            Analyse(0x18);
            Analyse(0x20);
            Analyse(0x28);
            Analyse(0x30);
            Analyse(0x38);
            Analyse(0x40);
            Analyse(0x48);
            Analyse(0x50);
            Analyse(0x58);
            Analyse(0x60);
            Analyse(0x100);
        }

        public void Analyse(uint start, bool force = false)
        {
            HashSet<uint> visited = new HashSet<uint>();
            Queue<uint> locations = new Queue<uint>();
            locations.Enqueue(start);

            while (locations.Count > 0)
            {
                uint bloc = locations.Dequeue();
                uint loc = bloc;

                while (true)
                {
                    bool skip;

                    if (force) skip = visited.Contains(loc);
                    else skip = Instructions.ContainsKey(loc);

                    if (skip) break;

                    Instruction i = CPU.Decode(loc);
                    if (i == null) break;

                    Instructions[loc] = i;
                    visited.Add(loc);

                    if (i.JumpLocation != null)
                    {
                        uint dest = i.JumpLocation.Value;
                        locations.Enqueue(dest);
                        if (!Labeller.Labels.ContainsKey(dest))
                            Labeller.Labels.Add(dest, $"auto_{dest:X6}");
                    }

                    if (i.IsEnd) break;
                    loc += i.TotalSize;
                }
            }
        }

        public IPortHandler FindHandler(Word op)
        {
            return Ports.FirstOrDefault(h => h.Handles(op));
        }

        private void SetupPorts()
        {
            Ports.Clear();
            Ports.Add(Labeller);
            Ports.Add(Namer);

            foreach (Type portType in Assembly.GetExecutingAssembly().GetTypes().Where(a => typeof(IPort).IsAssignableFrom(a) && !a.IsAbstract))
            {
                IPort port = (IPort)Activator.CreateInstance(portType, this);
                Ports.Add(port);
            }
        }

        private IMBC GetMBC(MemoryBankController mbc)
        {
            switch (mbc)
            {
                case MemoryBankController.None: return new NullMBC(this);
                case MemoryBankController.MBC1: return new MBC1(this);
                case MemoryBankController.MBC2: return new MBC2(this);
                case MemoryBankController.MBC5: return new MBC5(this);
                default: throw new NotImplementedException();
            }
        }
        
        public void AcquireROM()
        {
            using (var f = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ROM = new byte[f.Length];
                f.Read(ROM, 0, (int)f.Length);
            }

            Header = new RomHeader(UsefulExtensions.Slice(ROM, 0x100, 0x150));
            MBC = GetMBC(Header.MBC);
            Ports.Insert(0, MBC);
        }

        public void AddCustomOperand(uint location, Word word)
        {
            IInstruction inst;
            if (Instructions.ContainsKey(location)) inst = Instructions[location];
            else
            {
                inst = Instruction.DataWord(this, word, location);
                Instructions[location] = inst;
            }

            if (!inst.WordOperandIndex.HasValue)
            {
                // throw new InvalidDataException($"{inst} does not have a word operand!");
                return;
            }

            Word oldop = inst.Operands[inst.WordOperandIndex.Value] as Word;
            word.Indirect = oldop.Indirect;

            inst.Operands[inst.WordOperandIndex.Value] = word;
            CustomOperands[location] = word;
        }

        public void DeleteCustomOperand(uint location)
        {
            IInstruction inst = Instructions[location];
            if (!inst.IsReal) Instructions.Remove(location);
            else Instructions[location] = CPU.Decode(location);
        }

        public Word GuessFromContext(uint context, uint value)
        {
            uint cbank = context / 0x4000;
            uint vbank = value / 0x4000;
            Segment seg;

            if (vbank == 0) seg = ProjectExtensions.GetSegment(this, 0);
            else if (cbank > 0) seg = ProjectExtensions.GetSegment(this, context);
            else seg = null;

            return new Word(seg, value);
        }

        public void Export(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName))
            {
                sw.WriteLine($"; Disassembled from {Filename}\n");

                Header.WriteAsm(sw);

                foreach (var pair in Namer.Names)
                    sw.WriteLine($".DEFINE {pair.Value} ${pair.Key:X4}");

                uint o = 0;
                while (o < ROMSize)
                {
                    if (Labeller.Labels.ContainsKey(o))
                        sw.WriteLine($"\n{Labeller.Labels[o]}:");

                    if (Instructions.ContainsKey(o))
                    {
                        IInstruction inst = Instructions[o];
                        inst.WriteAsm(sw);
                        o += inst.TotalSize;

                        if (inst.IsEnd) sw.WriteLine("");
                    }
                    else
                    {
                        sw.WriteLine($".DB ${ROM[o]:X2}");
                        o++;
                    }
                }
            }
        }
    }
}
