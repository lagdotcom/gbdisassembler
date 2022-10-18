namespace Lag.NESLib.Port
{
    public class SPRIO : AbstractPort
    {
        public SPRIO(Nes dis) : base(dis, 0x2004, nameof(SPRIO)) { }
    }
}
