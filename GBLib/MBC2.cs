namespace GBLib
{
    public class MBC2 : IPortHandler
    {
        public MBC2()
        {
            RAMEnabled = false;
            ROMBank = 1;
        }

        public MBC2(Disassembler dis) : this()
        {
            Parent = dis;
        }

        public Disassembler Parent;
        public bool RAMEnabled;
        public int ROMBank;

        // this isn't technically correct
        public bool Handles(IOperand op) => op.Write && op.AbsoluteAddress < 0x3FFF;

        public string Identify(uint address)
        {
            if (address < 0x2000) return "[MBC2.RAMEnable]";
            else return "[MBC2.ROMBank]";
        }

        public void Apply(uint address, byte value)
        {
            if (address < 0x2000) EnableRAM(value);
            else SetROMBank(value);
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
