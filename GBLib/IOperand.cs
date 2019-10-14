namespace GBLib
{
    public interface IOperand
    {
        uint? AbsoluteAddress { get; }
        uint Value { get; }
        bool Read { get; }
        bool Write { get; }
        bool IsNumeric { get; }
        bool IsHex { get; }
        bool IsRegister { get; }

        char TypeKey { get; }
        uint? TypeValue { get; }
    }
}
