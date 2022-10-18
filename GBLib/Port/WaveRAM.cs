using Lag.DisassemblerLib;
using System;

namespace Lag.GBLib.Port
{
    public class WaveRAM : IPort
    {
        public WaveRAM(Gameboy dis)
        {
            Parent = dis;
        }

        public Gameboy Parent;
        public bool Handles(Word addr) => addr.Absolute >= 0xFF30 && addr.Absolute <= 0xFF3F;
        public string Identify(Word addr) => $"WaveRAM+${addr.Absolute - 0xFF30:X}";

        public void Apply(Word addr, byte value)
        {
            throw new NotImplementedException();
        }
    }
}
