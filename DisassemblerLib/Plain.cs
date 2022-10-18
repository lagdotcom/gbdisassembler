namespace Lag.DisassemblerLib
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
        public bool IsHex => false;
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public override string ToString() => $"{Value}";
    }
}
