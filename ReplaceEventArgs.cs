namespace GBDisassembler
{
    using GBLib;

    public class ReplaceEventArgs
    {
        public ReplaceEventArgs(uint location, int index, IOperand operand)
        {
            Location = location;
            Index = index;
            Operand = operand;
        }

        public uint Location { get; }
        public int Index { get; }
        public IOperand Operand { get; }
    }
}
