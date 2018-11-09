namespace GBLib.Operand
{
    class StackOffset : IOperand
    {
        public StackOffset(byte b)
        {
            Value = b;
        }

        public byte Value;
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;

        public override string ToString() => $"SP+{Value}";
    }
}
