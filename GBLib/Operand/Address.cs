namespace GBLib.Operand
{
    class Address : IOperand
    {
        public Address(uint i)
        {
            Value = i;
        }

        public uint Value;
        public bool Read { get; set; }
        public bool Write { get; set; }

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
