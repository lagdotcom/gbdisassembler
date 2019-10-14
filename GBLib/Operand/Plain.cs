namespace GBLib.Operand
{
    public class Plain : IOperand
    {
        public Plain(uint i = 0)
        {
            Value = i;
        }

        public char TypeKey => 'p';
        public uint? TypeValue => Value;

        public uint? AbsoluteAddress => null;
        public uint Value { get; }
        public bool Read => false;
        public bool Write => false;
        public bool IsAddress => false;
        public bool IsHex => false;
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public override string ToString() => $"{Value}";
    }
}
