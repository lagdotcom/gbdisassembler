namespace Lag.NESLib.Port
{
    public class VRAM1 : AbstractPort
    {
        public VRAM1(Nes dis) : base(dis, 0x2005, nameof(VRAM1)) { }
    }
}
