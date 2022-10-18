namespace Lag.NESLib.Port
{
    public class SNC : AbstractPort
    {
        public SNC(Nes dis) : base(dis, 0x400C, nameof(SNC)) { }
    }
}
