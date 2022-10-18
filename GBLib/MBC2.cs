using Lag.DisassemblerLib;
using System.Collections.Generic;

namespace Lag.GBLib
{
    public class MBC2 : IMBC
    {
        public const int BankSize = 0x4000;

        public MBC2()
        {
            RAMEnabled = false;
            ROMBank = 1;
        }

        public MBC2(Gameboy dis) : this()
        {
            Parent = dis;
        }

        public Gameboy Parent;
        public bool RAMEnabled;
        public int ROMBank;

        // this isn't technically correct
        public bool Handles(Word addr) => addr.Write && addr.Absolute < 0x3FFF;

        public string Identify(Word addr)
        {
            if (addr.Absolute < 0x2000) return "[MBC2.RAMEnable]";
            else return "[MBC2.ROMBank]";
        }

        public void Apply(Word addr, byte value)
        {
            if (addr.Absolute < 0x2000) EnableRAM(value);
            else SetROMBank(value);
        }

        public IEnumerable<Segment> Segments(RomHeader h)
        {
            List<Segment> segs = new List<Segment>();
            uint offset = 0;
            while (offset < h.ROM)
            {
                segs.Add(new Segment($"ROM{offset / BankSize:X2}", 0, BankSize, offset));
                offset += BankSize;
            }

            return segs;
        }

        private void EnableRAM(byte value)
        {
            RAMEnabled = value != 0;
        }

        private void SetROMBank(byte value)
        {
            ROMBank = value & 0x0f;
        }
    }
}
