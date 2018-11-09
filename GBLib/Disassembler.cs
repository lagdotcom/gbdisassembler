using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace GBLib
{
    public class Disassembler
    {
        public Disassembler(byte[] raw)
        {
            ROM = raw;

            Header = new RomHeader(Tool.Slice(raw, 0x100, 0x150));
            MBC = GetMBC(Header.MBC);
            CPU = new LR35902(this);

            Ports = new List<IPortHandler>
            {
                MBC
            };

            foreach (Type portType in Assembly.GetExecutingAssembly().GetTypes().Where(a => typeof(IPort).IsAssignableFrom(a) && !a.IsAbstract))
            {
                IPort port = (IPort)Activator.CreateInstance(portType, this);
                Ports.Add(port);
            }

            Decompiled = new Dictionary<uint, Instruction>();
            Labels = new Dictionary<uint, string>()
            {
                { 0x00, "RST00" },
                { 0x08, "RST08" },
                { 0x10, "RST10" },
                { 0x18, "RST18" },
                { 0x20, "RST20" },
                { 0x28, "RST28" },
                { 0x30, "RST30" },
                { 0x38, "RST38" },
                { 0x40, "VBI" },
                { 0x48, "LCDCI" },
                { 0x50, "TIMERI" },
                { 0x58, "SERI" },
                { 0x60, "JOYI" },
                { 0x100, "_ENTRY" },
            };
        }

        public LR35902 CPU;
        public RomHeader Header;
        public IPortHandler MBC;
        public List<IPortHandler> Ports;
        public byte[] ROM;

        public Dictionary<uint, Instruction> Decompiled;
        public Dictionary<uint, string> Labels;

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

        public void Analyse(uint start)
        {
            Queue<uint> locations = new Queue<uint>();
            locations.Enqueue(start);

            while (locations.Count > 0)
            {
                uint bloc = locations.Dequeue();
                uint loc = bloc;

                while (true)
                {
                    if (Decompiled.ContainsKey(loc)) break;

                    Instruction i = CPU.Decode(loc);
                    if (i == null) break;

                    Decompiled[loc] = i;

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

        private IPortHandler GetMBC(MemoryBankController mbc)
        {
            switch (mbc)
            {
                case MemoryBankController.MBC5: return new MBC5(this);
                default: throw new NotImplementedException();
            }
        }
    }
}
