namespace Lag.NESLib.Port
{
    public class VRAM2 : AbstractPort
    {
        public VRAM2(Nes dis) : base(dis, 0x2006, nameof(VRAM2)) { }
    }
}
