namespace GBLib
{
    public interface IPortHandler
    {
        bool Handles(IOperand op);
        string Identify(uint address);
        void Apply(uint address, byte value);
    }
}
