using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Lag.NESLib
{
    public class RomHeader
    {
        public RomHeader(BinaryReader br)
        {
            string magic = new string(br.ReadChars(4));
            if (magic != "NES\x1a") throw new InvalidDataException("Not an iNES header");

            PrgRomBanks = br.ReadByte();
            ChrRomBanks = br.ReadByte();

            Flags = 0;
            ReadFlags6(br.ReadByte());
            ReadFlags7(br.ReadByte());
            ReadFlags8(br.ReadByte());
            ReadFlags9(br.ReadByte());
            ReadFlags10(br.ReadByte());

            br.ReadBytes(5); // Padding

            if (Flags.HasFlag(HeaderFlags.HasTrainer))
                Trainer = br.ReadBytes(512);
        }

        public HeaderFlags Flags { get; private set; }
        public int Mapper { get; private set; }
        public byte[] Trainer { get; }

        public int PrgRamBanks { get; private set; }
        public int PrgRomBanks { get; }
        public int ChrRomBanks { get; }

        public int PrgRamSize => PrgRamBanks * 8 * 1024;
        public int PrgRomSize => PrgRomBanks * 16 * 1024;
        public int ChrRomSize => ChrRomBanks * 8 * 1024;

        private void ReadFlags6(byte b)
        {
            if ((b & 0b1) > 0) Flags |= HeaderFlags.VerticalMirroring;
            if ((b & 0b10) > 0) Flags |= HeaderFlags.HasPrgRam;
            if ((b & 0b100) > 0) Flags |= HeaderFlags.HasTrainer;
            if ((b & 0b1000) > 0) Flags |= HeaderFlags.FourScreenVram;

            Mapper += (b & 0b11110000) >> 4;
        }

        private void ReadFlags7(byte b)
        {
            if ((b & 0b1) > 0) Flags |= HeaderFlags.VsUnisystem;
            if ((b & 0b10) > 0) Flags |= HeaderFlags.PlayChoice10;
            if ((b & 0b1100) == 0b1100) Flags |= HeaderFlags.HeaderVersion2;

            Mapper += b & 0b11110000;
        }

        private void ReadFlags8(byte b)
        {
            PrgRamBanks = b == 0 ? 1 : b;
        }

        private void ReadFlags9(byte b)
        {
            if ((b & 0b1) > 0) Flags |= HeaderFlags.Pal;
        }

        private void ReadFlags10(byte b)
        {
            if ((b & 0b11) == 2) Flags |= HeaderFlags.Pal;
            else if ((b & 0b11) > 0) Flags |= HeaderFlags.Dual;
            if ((b & 0b10000) > 0) Flags |= HeaderFlags.HasPrgRam;
            if ((b & 0b100000) > 0) Flags |= HeaderFlags.BusConflicts;
        }
    }
}
