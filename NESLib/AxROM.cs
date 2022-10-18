using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;

namespace Lag.NESLib
{
    class AxROM : IMapper
    {
        public const int BankSize = 0x8000;

        public AxROM() { }
        public AxROM(Nes dis)
        {
            Parent = dis;
        }

        public Nes Parent { get; }

        public void Apply(Word op, byte value)
        {
            throw new NotImplementedException();
        }

        public bool Handles(Word op) => op.ROM && op.Write && op.Offset < BankSize;

        public string Identify(Word op) => "[AxROM.Bank]";

        public IEnumerable<Segment> Segments(RomHeader h)
        {
            List<Segment> segs = new List<Segment>();
            int banks = h.PrgRomBanks / 2;
            
            for (uint bank = 0; bank < banks; bank++)
                segs.Add(new Segment($"ROM{bank:X1}", 0x8000, BankSize, BankSize * bank));

            return segs;
        }
    }
}
