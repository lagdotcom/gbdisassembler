using System;
using System.Collections.Generic;

namespace GBLib
{
    public class Namer : IPortHandler
    {
        public Namer()
        {
            Names = new Dictionary<uint, string>();
        }

        public Namer(Disassembler dis) : this()
        {
            Parent = dis;
        }

        public Disassembler Parent;
        public Dictionary<uint, string> Names;

        public bool Handles(IOperand op) => (op.Read || op.Write) && op.AbsoluteAddress.HasValue && Names.ContainsKey(op.AbsoluteAddress.Value);
        public string Identify(uint address) => Names[address];

        public void Apply(uint address, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
