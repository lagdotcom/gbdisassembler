﻿namespace Lag.GBLib.Port
{
    public class KEY1 : AbstractPort
    {
        public KEY1(Gameboy dis) : base(dis, 0xFF4D, nameof(KEY1)) { }
    }
}
