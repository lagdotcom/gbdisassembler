namespace GBLib.Operand
{
    public class ByteValue : IOperand
    {
        public ByteValue(byte b = 0)
        {
            Value = b;
        }

        public char TypeKey => 'b';
        public uint? TypeValue => Value;

        public uint Value { get; }
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;
        public bool IsHex => true;
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public override string ToString() => $"${Value:X2}";
    }
}
