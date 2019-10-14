namespace GBLib.Operand
{
    public class IndirectAddress : IOperand
    {
        public IndirectAddress(uint i = 0)
        {
            Value = i;
        }

        public char TypeKey => 'I';
        public uint? TypeValue => Value;

        public uint Value { get; }
        
        // TODO
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;
        public bool IsAddress => true;
        public bool IsHex => true;
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public override string ToString() => $"(${Value:X4})";
    }
}
