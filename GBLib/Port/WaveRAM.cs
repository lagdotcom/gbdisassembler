using System;

namespace GBLib.Port
{
    public class WaveRAM : IPort
    {
        public WaveRAM(Disassembler dis)
        {
            Parent = dis;
        }

        public Disassembler Parent;
        public bool Handles(IOperand op) => op.AbsoluteAddress >= 0xFF30 && op.AbsoluteAddress <= 0xFF3F;
        public string Identify(uint address) => $"[WaveRAM.{address - 0xFF30:X}]";

        public void Apply(uint address, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
