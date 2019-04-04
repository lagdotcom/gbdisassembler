namespace GBLib
{
    public class MBC5 : IPortHandler
    {
        public MBC5()
        {
            RAMEnabled = false;
            RAMBank = 0;
            ROMBankLo = 1;
            ROMBankHi = 0;
        }

        public MBC5(Disassembler dis) : this()
        {
            Parent = dis;
        }

        public Disassembler Parent;
        public bool RAMEnabled;
        public int RAMBank;
        public byte ROMBankLo;
        public byte ROMBankHi;
        public int ROMBank => ROMBankHi * 256 + ROMBankLo;

        public bool Handles(IOperand op) => op.Write && op.AbsoluteAddress < 0x6000;

        public string Identify(uint address)
        {
            if (address < 0x2000) return "[MBC5.RAMEnable]";
            else if (address < 0x3000) return "[MBC5.ROMBankLo]";
            else if (address < 0x4000) return "[MBC5.ROMBankHi]";
            else return "[MBC5.RAMBank]";
        }

        public void Apply(uint address, byte value)
        {
            if (address < 0x2000) EnableRAM(value);
            else if (address < 0x3000) SetROMBankLo(value);
            else if (address < 0x4000) SetROMBankHi(value);
            else SetRAMBank(value);
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
