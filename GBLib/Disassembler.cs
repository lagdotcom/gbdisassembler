using System;
using System.Collections.Generic;

namespace GBLib
{
    public class Disassembler
    {
        public Disassembler(byte[] raw)
        {
            Raw = raw;

            CodeLocations = new List<int>();
            CPU = new LR35902(Raw);
            Header = new RomHeader(Tool.Slice(raw, 0x100, 0x150));
        }

        public List<int> CodeLocations;
        public LR35902 CPU;
        public RomHeader Header;
        public byte[] Raw;

        public void Analyse(int start)
        {
            Queue<int> locations = new Queue<int>();
            locations.Enqueue(start);

            while (locations.Count > 0)
            {
                int loc = locations.Dequeue();
                if (CodeLocations.Contains(loc)) continue;

                Instruction i = CPU.Decode(loc);
                if (i == null)
                {
                    Console.WriteLine($"{loc:X6}: .db {Raw[loc]:X2}");
                    continue;
                }

                MarkAsCode(loc);
                Console.WriteLine(i);

                if (i.JumpLocation != null)
                    locations.Enqueue(i.JumpLocation.Value);

                if (!i.IsEnd)
                    locations.Enqueue(loc + i.TotalSize);
            }
        }

        private void MarkAsCode(int location)
        {
            CodeLocations.Add(location);
        }
    }
}
