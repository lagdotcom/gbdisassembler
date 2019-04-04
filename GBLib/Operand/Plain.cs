namespace GBLib.Operand
{
    class Plain : IOperand
    {
        public Plain(int i = 0)
        {
            Value = i;
        }

        public int Value;
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;
        public bool IsHex => false;
        public bool IsNumeric => false;
        public bool IsRegister => false;

        public override string ToString() => $"{Value}";
    }
}
