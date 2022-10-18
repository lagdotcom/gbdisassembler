using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;

namespace Lag.GBLib
{
    public class MBC1 : IMBC
    {
        public const int BankSize = 0x4000;

        public MBC1()
        {
            RAMEnabled = false;
            ROMBank = 1;
        }

        public MBC1(Gameboy dis) : this()
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
            if (addr.Absolute < 0x2000) return "[MBC1.RAMEnable]";
            else if (addr.Absolute < 0x4000) return "[MBC1.ROMBank]";
            else if (addr.Absolute < 0x6000) return "[MBC1.Special]";
            else return "[MBC1.SpecialMode]";
        }

        public void Apply(Word addr, byte value)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Segment> Segments(RomHeader h)
        {
            List<Segment> segs = new List<Segment>();
            uint offset = 0;
            while (offset < h.ROM)
            {
                segs.Add(new Segment($"ROM{offset / BankSize:X3}", 0, BankSize, offset));
                offset += BankSize;
            }

            return segs;
        }

    }
}
