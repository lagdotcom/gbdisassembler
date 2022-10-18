using System;
using System.Collections.Generic;

namespace Lag.DisassemblerLib
{
    public class Namer : IPortHandler
    {
        public Namer()
        {
            Names = new Dictionary<uint, string>();
        }

        public Namer(IProject dis) : this()
        {
            Parent = dis;
        }

        public IProject Parent;
        public Dictionary<uint, string> Names;

        public bool Handles(Word addr) => addr.RAM && Names.ContainsKey(addr.Offset);
        public string Identify(Word addr) => Names[addr.Offset];

        public void Apply(Word addr, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
