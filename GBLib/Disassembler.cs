using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace GBLib
{
    public class Disassembler
    {
        public Disassembler()
        {
            Labeller = new Labeller(this);
            Namer = new Namer(this);
            CPU = new LR35902(this);
            Ports = new List<IPortHandler>();
            Instructions = new Dictionary<uint, Instruction>();

            SetupPorts();
        }

        public Disassembler(string filename) : this()
        {
            Filename = filename;
            AcquireROM();
        }

        public string Filename;
        public LR35902 CPU;
        public RomHeader Header;
        public List<IPortHandler> Ports;
        public byte[] ROM;
        public IPortHandler MBC;

        public Dictionary<uint, Instruction> Instructions;
        public Labeller Labeller;
        public Namer Namer;

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
                    bool skip = false;

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

        public IPortHandler FindHandler(IOperand op)
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

        private IPortHandler GetMBC(MemoryBankController mbc)
        {
            switch (mbc)
            {
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

            Header = new RomHeader(Tool.Slice(ROM, 0x100, 0x150));
            MBC = GetMBC(Header.MBC);
            Ports.Insert(0, MBC);
        }
    }
}
