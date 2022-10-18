namespace Lag.NESLib.Port
{
    public class S2FT : AbstractPort
    {
        public S2FT(Nes dis) : base(dis, 0x4006, nameof(S2FT)) { }
    }
}
