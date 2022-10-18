namespace Lag.NESLib.Port
{
    public class S1FT : AbstractPort
    {
        public S1FT(Nes dis) : base(dis, 0x4002, nameof(S1FT)) { }
    }
}
