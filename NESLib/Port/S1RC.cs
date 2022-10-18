namespace Lag.NESLib.Port
{
    public class S1RC : AbstractPort
    {
        public S1RC(Nes dis) : base(dis, 0x4001, nameof(S1RC)) { }
    }
}
