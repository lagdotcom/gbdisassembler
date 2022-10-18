using Lag.DisassemblerLib;
using System;

namespace Lag.NESLib.Port
{
    public abstract class AbstractPort : IPort
    {
        public AbstractPort(Nes dis, uint address, string name)
        {
            Parent = dis;
            Address = address;
            Name = name;
        }

        public Nes Parent;
        public uint Address;
        public string Name;

        public bool Handles(Word addr) => addr.RAM && addr.Absolute == Address;
        public string Identify(Word addr) => $"[{Name}]";

        public virtual void Apply(Word addr, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
