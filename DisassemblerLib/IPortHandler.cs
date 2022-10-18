
namespace Lag.DisassemblerLib
{
    public interface IPortHandler
    {
        bool Handles(Word op);
        string Identify(Word op);
        void Apply(Word op, byte value);
    }
}
