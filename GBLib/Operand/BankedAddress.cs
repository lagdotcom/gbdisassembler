namespace GBLib.Operand
{
    using System.Globalization;

    public class BankedAddress : IOperand
    {
        public const uint BankSize = 0x4000;

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

        public BankedAddress(uint? bank, uint offset)
        {
            Bank = bank;
            Offset = offset % BankSize;
        }
        
        public static BankedAddress Parse(string spec)
        {
            uint bank = uint.Parse(spec.Substring(0, 2), NumberStyles.HexNumber);
            uint offset = uint.Parse(spec.Substring(3, 4), NumberStyles.HexNumber);
            return new BankedAddress { Bank = bank, Offset = offset };
        }

        public static BankedAddress GuessFromContext(uint location, uint i)
        {
            if (i >= BankSize)
            {
                if (location < BankSize) return new BankedAddress(null, i);   // unknown destination bank
                return new BankedAddress(location / BankSize, i);
            }

            return new BankedAddress(i);
        }

        public char TypeKey => 'B';
        public uint? TypeValue => Value;

        public bool Read => false;
        public bool Write => false;
        public bool IsHex => true;
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public uint? Bank;
        public uint Offset;
        public uint? AbsoluteAddress => Bank * BankSize + Offset;
        public uint Value => AbsoluteAddress ?? BankSize + Offset;

        public override string ToString() => Bank.HasValue ? $"{Bank:X2}:{Offset:X4}" : $"??:{Offset:X4}";
    }
}
