namespace GBLib.Operand
{
    public class Address : IOperand
    {
        public Address(uint i = 0)
        {
            Value = i;
        }

        public char TypeKey => 'A';
        public uint? TypeValue => Value;

        public uint Value { get; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool IsHex => true;
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public uint? AbsoluteAddress => IO ? Value : (uint?)null;
        public bool IO => Read || Write;

        public Address SetRead()
        {
            Read = true;
            return this;
        }
        public Address SetWrite()
        {
            Write = true;
            return this;
        }

        public override string ToString() => $"${Value:X4}";
    }
}
