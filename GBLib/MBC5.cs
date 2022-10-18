using Lag.DisassemblerLib;
using System.Collections.Generic;

namespace Lag.GBLib
{
    public class MBC5 : IMBC
    {
        public const int BankSize = 0x4000;

        public MBC5()
        {
            RAMEnabled = false;
            RAMBank = 0;
            ROMBankLo = 1;
            ROMBankHi = 0;
        }

        public MBC5(Gameboy dis) : this()
        {
            Parent = dis;
        }

        public Gameboy Parent;
        public bool RAMEnabled;
        public int RAMBank;
        public byte ROMBankLo;
        public byte ROMBankHi;
        public int ROMBank => ROMBankHi * 256 + ROMBankLo;

        public bool Handles(Word addr) => addr.Write && addr.Absolute < 0x6000;

        public string Identify(Word addr)
        {
            if (addr.Absolute < 0x2000) return "[MBC5.RAMEnable]";
            else if (addr.Absolute < 0x3000) return "[MBC5.ROMBankLo]";
            else if (addr.Absolute < 0x4000) return "[MBC5.ROMBankHi]";
            else return "[MBC5.RAMBank]";
        }

        public void Apply(Word addr, byte value)
        {
            if (addr.Absolute < 0x2000) EnableRAM(value);
            else if (addr.Absolute < 0x3000) SetROMBankLo(value);
            else if (addr.Absolute < 0x4000) SetROMBankHi(value);
            else SetRAMBank(value);
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

        private void EnableRAM(byte value)
        {
            RAMEnabled = value == 0x0a;
        }

        private void SetROMBankLo(byte value)
        {
            ROMBankLo = value;
        }

        private void SetROMBankHi(byte value)
        {
            ROMBankHi = (byte)(value & 0x0001);
        }

        private void SetRAMBank(byte value)
        {
            RAMBank = (byte)(value & 0xFF);
        }
    }
}
