namespace GBLib.Operand
{
    class IndirectAddress : IOperand
    {
        public IndirectAddress(uint i = 0)
        {
            Value = i;
        }

        public uint Value;
        
        // TODO
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;

        public override string ToString() => $"(${Value:X4})";
    }
}
