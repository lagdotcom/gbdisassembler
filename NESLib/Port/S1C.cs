namespace Lag.NESLib.Port
{
    public class S1C : AbstractPort
    {
        public S1C(Nes dis) : base(dis, 0x4000, nameof(S1C)) { }
    }
}
