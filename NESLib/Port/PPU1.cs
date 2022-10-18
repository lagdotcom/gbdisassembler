namespace Lag.NESLib.Port
{
    public class PPU1 : AbstractPort
    {
        public PPU1(Nes dis) : base(dis, 0x2000, nameof(PPU1)) { }
    }
}
