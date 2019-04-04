namespace GBLib.Operand
{
    class ByteValue : IOperand
    {
        public ByteValue(byte b = 0)
        {
            Value = b;
        }

        public byte Value;
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;
        public bool IsHex => true;
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public override string ToString() => $"${Value:X2}";
    }
}
