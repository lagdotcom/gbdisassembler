namespace GBLib.Port
{
    class IE : AbstractPort
    {
        public IE(Disassembler dis) : base(dis, 0xFFFF, nameof(IE))
        {
            Flags = 0;
        }

        public byte Flags;
        public bool VBlank => (Flags & 0x01) > 0;
        public bool LCDStat => (Flags & 0x02) > 0;
        public bool Timer => (Flags & 0x04) > 0;
        public bool Serial => (Flags & 0x08) > 0;
        public bool Joypad => (Flags & 0x10) > 0;

        override public void Apply(uint address, byte value)
        {
            Flags = value;
        }
    }
}
