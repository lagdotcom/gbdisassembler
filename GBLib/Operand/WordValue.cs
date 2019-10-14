namespace GBLib.Operand
{
    public class WordValue : IOperand
    {
        public WordValue(uint v = 0)
        {
            Value = v;
        }

        public char TypeKey => 'w';
        public uint? TypeValue => Value;

        public uint Value { get; }
        public uint? AbsoluteAddress => Value;
        public bool Read => false;
        public bool Write => false;
        public bool IsHex => true;
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public override string ToString() => $"${Value:X4}";
    }
}
