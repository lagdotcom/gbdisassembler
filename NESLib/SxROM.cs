using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;

namespace Lag.NESLib
{
    class SxROM : IMapper
    {
        public const int PrgBankSize = 0x4000;
        public const int ChrBankSize = 0x1000;

        public SxROM() { }
        public SxROM(Nes dis)
        {
            Parent = dis;
        }

        public Nes Parent { get; }

        public void Apply(Word op, byte value)
        {
            throw new NotImplementedException();
        }

        public bool Handles(Word op) => op.ROM && op.Write && op.Offset >= 0x8000;

        public string Identify(Word op) => "[MMC1]";

        public IEnumerable<Segment> Segments(RomHeader h)
        {
            List<Segment> segs = new List<Segment>();

            int prgs = h.PrgRomSize / PrgBankSize;
            for (uint bank = 0; bank < prgs; bank++)
            {
                // TODO: This is a major assumption...
                bool last = bank == prgs - 1;
                uint offset = last ? (uint)0xC000 : 0x8000;
                segs.Add(new Segment($"ROM{bank:X2}", offset, PrgBankSize, PrgBankSize * bank));
            }

            int chrs = h.ChrRomSize / ChrBankSize;
            for (uint bank = 0; bank < chrs; bank++)
                segs.Add(new Segment($"CHR{bank:X2}", 0x0000, ChrBankSize, (uint)(ChrBankSize * bank + h.PrgRomSize)));

            return segs;
        }
    }
}
