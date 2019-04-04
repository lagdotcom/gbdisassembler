namespace GBLib
{
    public interface IOperand
    {
        uint? AbsoluteAddress { get; }
        bool Read { get; }
        bool Write { get; }
        bool IsNumeric { get; }
        bool IsHex { get; }
        bool IsRegister { get; }
    }
}
