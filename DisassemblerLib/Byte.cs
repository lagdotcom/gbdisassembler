namespace Lag.DisassemblerLib
{
    public class Byte : IOperand
    {
        public Byte(byte b = 0)
        {
            Value = b;
        }

        public bool IsHex { get; set; }
        public bool IsNumeric => true;
        public bool IsRegister => false;
        public byte Value { get; }

        public override string ToString() => IsHex ? $"${Value:X2}" : Value.ToString();
    }
}
