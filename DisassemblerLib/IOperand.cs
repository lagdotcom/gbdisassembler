namespace Lag.DisassemblerLib
{
    public interface IOperand
    {
        bool IsHex { get; }
        bool IsNumeric { get; }
        bool IsRegister { get; }
    }
}
