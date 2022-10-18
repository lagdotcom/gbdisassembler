using Lag.DisassemblerLib;

namespace Lag.Disassembler
{
    public class ReplaceEventArgs
    {
        public ReplaceEventArgs(uint location, Word operand)
        {
            Location = location;
            Operand = operand;
        }

        public uint Location { get; }
        public Word Operand { get; }
    }
}
