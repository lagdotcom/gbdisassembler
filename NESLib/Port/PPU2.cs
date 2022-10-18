namespace Lag.NESLib.Port
{
    public class PPU2 : AbstractPort
    {
        public PPU2(Nes dis) : base(dis, 0x2001, nameof(PPU2)) { }
    }
}
