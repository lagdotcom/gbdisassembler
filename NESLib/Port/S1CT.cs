namespace Lag.NESLib.Port
{
    public class S1CT : AbstractPort
    {
        public S1CT(Nes dis) : base(dis, 0x4003, nameof(S1CT)) { }
    }
}
