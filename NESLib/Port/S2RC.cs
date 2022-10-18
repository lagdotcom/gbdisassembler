namespace Lag.NESLib.Port
{
    public class S2RC : AbstractPort
    {
        public S2RC(Nes dis) : base(dis, 0x4005, nameof(S2RC)) { }
    }
}
