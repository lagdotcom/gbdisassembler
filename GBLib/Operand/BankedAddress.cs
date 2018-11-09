namespace GBLib.Operand
{
    public class BankedAddress : IOperand
    {
        const uint BankSize = 0x4000;

        public BankedAddress()
        {
            Bank = 0;
            Offset = 0;
        }

        public BankedAddress(uint absolute)
        {
            Bank = absolute / BankSize;
            Offset = absolute % BankSize;
        }

        public BankedAddress(uint absolute, uint bankOverride)
        {
            Bank = bankOverride;
            Offset = absolute % BankSize;
        }

        public bool Read => false;
        public bool Write => false;

        public uint Bank;
        public uint Offset;
        public uint? AbsoluteAddress => Bank * BankSize + Offset;

        public override string ToString() => $"{Bank:X2}:{Offset:X4}";
    }
}
