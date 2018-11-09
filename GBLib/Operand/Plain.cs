namespace GBLib.Operand
{
    class Plain : IOperand
    {
        public Plain(int i)
        {
            Value = i;
        }

        public int Value;
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;

        public override string ToString() => $"{Value}";
    }
}
