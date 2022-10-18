using System;
using System.Collections.Generic;

namespace Lag.DisassemblerLib
{
    public class Labeller : IPortHandler
    {
        public Labeller()
        {
            StandardLabels();
        }

        public Labeller(IProject dis) : this()
        {
            Parent = dis;
        }

        public IProject Parent;
        public Dictionary<uint, string> Labels;

        public bool Handles(Word addr) => addr.ROM && Labels.ContainsKey(addr.Absolute);
        public string Identify(Word addr) => Labels[addr.Absolute];

        public void Apply(Word address, byte value)
        {
            throw new NotImplementedException();
        }

        protected virtual void StandardLabels()
        {
            Labels = new Dictionary<uint, string>();
        }
    }
}
