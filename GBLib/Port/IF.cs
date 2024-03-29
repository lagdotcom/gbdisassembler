﻿using Lag.DisassemblerLib;

namespace Lag.GBLib.Port
{
    class IF : AbstractPort
    {
        public IF(Gameboy dis) : base(dis, 0xFF0F, nameof(IF))
        {
            Flags = 0;
        }

        public byte Flags;
        public bool VBlank => (Flags & 0x01) > 0;
        public bool LCDStat => (Flags & 0x02) > 0;
        public bool Timer => (Flags & 0x04) > 0;
        public bool Serial => (Flags & 0x08) > 0;
        public bool Joypad => (Flags & 0x10) > 0;

        override public void Apply(Word addr, byte value)
        {
            Flags = value;
        }
    }
}
