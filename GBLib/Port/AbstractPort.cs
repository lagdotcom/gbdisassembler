using System;

namespace GBLib.Port
{
    public abstract class AbstractPort : IPort
    {
        public AbstractPort(Disassembler dis, uint address, string name)
        {
            Parent = dis;
            Address = address;
            Name = name;
        }

        public Disassembler Parent;
        public uint Address;
        public string Name;

        public bool Handles(IOperand op) => (op.Read || op.Write) && op.AbsoluteAddress == Address;
        public string Identify(uint address) => $"[{Name}]";

        public virtual void Apply(uint address, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
