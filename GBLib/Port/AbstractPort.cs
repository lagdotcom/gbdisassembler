using Lag.DisassemblerLib;
using System;

namespace Lag.GBLib.Port
{
    public abstract class AbstractPort : IPort
    {
        public AbstractPort(Gameboy dis, uint address, string name)
        {
            Parent = dis;
            Address = address;
            Name = name;
        }

        public Gameboy Parent;
        public uint Address;
        public string Name;

        public bool Handles(Word addr) => addr.RAM && addr.Absolute == Address;
        public string Identify(Word addr) => Name;

        public virtual void Apply(Word addr, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
