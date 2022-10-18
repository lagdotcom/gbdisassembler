using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Lag.NESLib
{
    public class Nes : IProject
    {
        public const string ProjectMarker = "NESDPROJ@RP2A03:";

        public Nes()
        {
            Comments = new Dictionary<uint, string>();
            CPU = new RP2A03(this);
            CustomOperands = new Dictionary<uint, Word>();
            Instructions = new Dictionary<uint, IInstruction>();
            Contexts = new Dictionary<uint, InstructionContext>();
            Labeller = new Labeller(this);
            Namer = new Namer(this);
            Ports = new List<IPortHandler>();

            SetupPorts();
        }

        public Nes(string path) : this()
        {
            Filename = path;
            AcquireROM();
        }

        public RP2A03 CPU { get; }

        public RomHeader Header { get; private set; }
        public IMapper Mapper { get; private set; }
        public IEnumerable<Segment> Segments => Mapper.Segments(Header);
        public IEnumerable<Segment> RAMSegments => throw new NotImplementedException();

        public List<IPortHandler> Ports { get; }

        private IMapper GetMapper(MapperID m)
        {
            switch (m)
            {
                case MapperID.SxROM: return new SxROM(this);
                case MapperID.AxROM: return new AxROM(this);
                default: throw new NotImplementedException();
            }
        }
        public Word GuessFromContext(uint context, uint value)
        {
            return new Word(ProjectExtensions.GetSegment(this, context), value);
        }

        #region IProject
        public Dictionary<uint, string> Comments { get; private set; }

        public Dictionary<uint, Word> CustomOperands { get; private set; }

        public Dictionary<uint, IInstruction> Instructions { get; private set; }
        public Dictionary<uint, InstructionContext> Contexts { get; private set; }

        public Labeller Labeller { get; private set; }

        public string Marker => ProjectMarker;

        public Namer Namer { get; private set; }

        public byte[] ROM { get; private set; }

        public uint ROMSize => (uint)(Header.PrgRomSize + Header.ChrRomSize);

        public int MaxOpSize => 3;

        public string Filename { get; set; }

        public void AcquireROM()
        {
            using (var f = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                Header = new RomHeader(new BinaryReader(f));

                long remaining = f.Length - f.Position;
                ROM = new byte[remaining];
                f.Read(ROM, 0, (int)remaining);
            }

            Mapper = GetMapper((MapperID)Header.Mapper);
            Ports.Insert(0, Mapper);
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
                //throw new InvalidDataException($"{inst} does not have a word operand!");
                // TODO
                return;
            }

            Word oldop = inst.Operands[inst.WordOperandIndex.Value] as Word;
            word.Indirect = oldop.Indirect;

            inst.Operands[inst.WordOperandIndex.Value] = word;
            CustomOperands[location] = word;
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
                        locations.Enqueue(i.JumpLocation.Value);

                    if (i.IsEnd) break;
                    loc += i.TotalSize;
                }
            }
        }

        public void DeleteCustomOperand(uint address)
        {
            IInstruction inst = Instructions[address];
            if (!inst.IsReal) Instructions.Remove(address);
            else Instructions[address] = CPU.Decode(address);
        }

        public void Export(string fileName)
        {
            throw new NotImplementedException();
        }

        public IPortHandler FindHandler(Word op) => Ports.FirstOrDefault(h => h.Handles(op));

        public IInstruction Parse(uint address) => CPU.Decode(address);

        public void StandardAnalysis()
        {
            Analyse(0);
        }
        #endregion

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
    }
}
