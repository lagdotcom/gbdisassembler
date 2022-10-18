namespace Lag.GBLib.Port
{
    public class LCDC : AbstractPort
    {
        public LCDC(Gameboy dis) : base(dis, 0xFF40, nameof(LCDC)) { }
    }
}
