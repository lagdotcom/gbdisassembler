namespace Lag.NESLib.Port
{
    public class S2CT : AbstractPort
    {
        public S2CT(Nes dis) : base(dis, 0x4007, nameof(S2CT)) { }
    }
}
