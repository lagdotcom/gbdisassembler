namespace Lag.NESLib.Port
{
    public class S2C : AbstractPort
    {
        public S2C(Nes dis) : base(dis, 0x4004, nameof(S2C)) { }
    }
}
