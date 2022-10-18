namespace Lag.NESLib.Port
{
    public class VRAMIO : AbstractPort
    {
        public VRAMIO(Nes dis) : base(dis, 0x2007, nameof(VRAMIO)) { }
    }
}
