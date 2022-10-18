using Lag.DisassemblerLib;

namespace Lag.NESLib
{
    public class RegOp : IOperand
    {
        public RegOp(string reg)
        {
            Name = reg;
        }

        public string Name { get; }

        public bool IsHex => false;

        public bool IsNumeric => false;

        public bool IsRegister => true;

        public override string ToString() => Name;
    }
}