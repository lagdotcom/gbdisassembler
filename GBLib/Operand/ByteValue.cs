namespace GBLib.Operand
{
    class ByteValue : IOperand
    {
        public ByteValue(byte b)
        {
            Value = b;
        }

        public byte Value;
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;

        public override string ToString() => $"${Value:X2}";
    }
}
