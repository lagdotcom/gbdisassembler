﻿using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lag.GBLib
{
    public class RomHeader
    {
        public RomHeader() { }

        public RomHeader(byte[] raw) : this()
        {
            CartridgeType = raw[0x47];
            RAMSize = raw[0x49];
            DestinationByte = raw[0x4A];

            CGB = DetermineCGB(raw[0x43]);
            SGB = DetermineSGB(raw[0x46]);
            Logo = raw.Slice(0x04, 0x34);
            MBC = DetermineMBC(raw[0x47]);
            ROM = DetermineROM(raw[0x48]);
            RAM = DetermineRAM(raw[0x49]);
            Destination = DetermineDestination(raw[0x4A]);
            Version = raw[0x4C];
            HeaderChecksum = raw[0x4D];
            GlobalChecksum = BitConverter.ToUInt16(raw, 0x4e);

            if (CGB == CGBCompatibility.NonCGB)
            {
                Title = raw.Slice(0x34, 0x44).ToAscii();
            }
            else
            {
                Title = raw.Slice(0x34, 0x3F).ToAscii();
                Manufacturer = raw.Slice(0x3F, 0x43).ToAscii();
            }

            if (raw[0x4b] == 0x33)
            {
                NewLicensee = raw.Slice(0x44, 0x46).ToAscii();
            }
            else
            {
                OldLicensee = raw[0x4b];
            }
        }

        public byte[] Logo;
        public bool LogoValid => Logo.SequenceEqual(Consts.NintendoLogo);
        public string Title;
        public string Manufacturer;
        public CGBCompatibility CGB;
        public string NewLicensee;
        public bool SGB;
        public MemoryBankController MBC;
        public uint ROM;
        public uint RAM;
        public DestinationCode Destination;
        public byte? OldLicensee;
        public byte Version;
        public byte HeaderChecksum;
        public ushort GlobalChecksum;

        public byte CartridgeType;
        public byte RAMSize;
        public byte DestinationByte;

        private static CGBCompatibility DetermineCGB(byte b)
        {
            switch (b)
            {
                case 0x80: return CGBCompatibility.CGB;
                case 0xC0: return CGBCompatibility.CGBOnly;
                default: return CGBCompatibility.NonCGB;
            }
        }

        private static bool DetermineSGB(byte b)
        {
            return b == 0x03;
        }

        private static MemoryBankController DetermineMBC(byte b)
        {
            switch (b)
            {
                case 0x01: case 0x02: case 0x03:
                    return MemoryBankController.MBC1;

                case 0x05: case 0x06:
                    return MemoryBankController.MBC2;

                case 0x0B: case 0x0C:
                    return MemoryBankController.MMM01;

                case 0x0F: case 0x10: case 0x11: case 0x12: case 0x13:
                    return MemoryBankController.MBC3;

                case 0x19: case 0x1A: case 0x1B: case 0x1C: case 0x1D: case 0x1E:
                    return MemoryBankController.MBC5;

                case 0x20: return MemoryBankController.MBC6;
                case 0x22: return MemoryBankController.MBC7;
                case 0xFE: return MemoryBankController.HuC3;
                case 0xFF: return MemoryBankController.HuC1;

                default: return MemoryBankController.None;
            }
        }

        private static uint DetermineROM(byte b)
        {
            switch(b)
            {
                case 0x00: return 1024 * 32;
                case 0x01: return 1024 * 64;
                case 0x02: return 1024 * 128;
                case 0x03: return 1024 * 256;
                case 0x04: return 1024 * 512;
                case 0x05: return 1024 * 1024;
                case 0x06: return 1024 * 1024 * 2;
                case 0x07: return 1024 * 1024 * 4;
                case 0x08: return 1024 * 1024 * 8;
                case 0x52: return 1024 * 1152;
                case 0x53: return 1024 * 1280;
                case 0x54: return 1024 * 1536;
                default: return 0;
            }
        }

        private static uint DetermineRAM(byte b)
        {
            switch (b)
            {
                case 0x01: return 1024 * 2;
                case 0x02: return 1024 * 8;
                case 0x03: return 1024 * 32;
                case 0x04: return 1024 * 128;
                case 0x05: return 1024 * 64;
                default: return 0;
            }
        }

        private static DestinationCode DetermineDestination(byte b) => b == 0 ? DestinationCode.Japan : DestinationCode.NonJapan;

        public override string ToString()
        {
            List<string> lines = new List<string>();
            List<string> parts = new List<string>();

            parts.Add($"; '{Title}'");
            parts.Add($"v{Version}");
            parts.Add($"({Destination})");
            parts.Add($"({CGB})");
            if (SGB) parts.Add("(SGB)");
            if (!LogoValid) parts.Add("(BAD LOGO)");
            lines.Add(string.Join(" ", parts));
            parts.Clear();

            parts.Add($"; Manufacturer: {Manufacturer}");
            if (OldLicensee.HasValue) parts.Add($"Licensee: {OldLicensee}");
            else parts.Add($"Licensee: {NewLicensee}");
            lines.Add(string.Join(" ", parts));
            parts.Clear();

            lines.Add($"; MBC:{MBC} ROM:{ROM / 1024}KB RAM:{RAM / 1024}KB");

            return string.Join("\n", lines);
        }

        public void WriteAsm(StreamWriter sw)
        {
            if (CGB == CGBCompatibility.NonCGB)
                sw.WriteLine(".INCLUDE \"gb_hardware.i\"");
            else
                sw.WriteLine(".INCLUDE \"cgb_hardware.i\"");

            sw.WriteLine("\n.GBHEADER");
            sw.WriteLine($"\tNAME \"{Title}\"");
            if (OldLicensee.HasValue)
                sw.WriteLine($"\tLICENSEECODEOLD ${OldLicensee:X2}");
            else
                sw.WriteLine($"\tLICENSEECOLDNEW \"{NewLicensee}\"");
            sw.WriteLine($"\tCARTRIDGETYPE ${CartridgeType:X2}");
            sw.WriteLine($"\tRAMSIZE ${RAMSize:X2}");
            sw.WriteLine($"\tCOUNTRYCODE ${DestinationByte:X2}");
            sw.WriteLine("\tNINTENDOLOGO");
            sw.WriteLine($"\tVERSION ${Version:X2}");
            sw.WriteLine($"\t{GetRomAsm()}\n");
            
            sw.WriteLine(".ENDGB\n");
        }

        private string GetRomAsm()
        {
            switch (CGB)
            {
                case CGBCompatibility.CGB: return "ROMGBC";
                case CGBCompatibility.CGBOnly: return "ROMGBCONLY";
                case CGBCompatibility.NonCGB: return "ROMDMG";
                default: return $"; Unknown CGB state: {CGB}";
            }
        }
    }
}
