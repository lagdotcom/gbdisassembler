﻿namespace GBLib.Port
{
    public class HDMA4 : AbstractPort
    {
        public HDMA4(Disassembler dis) : base(dis, 0xFF54, nameof(HDMA4)) { }
    }
}
