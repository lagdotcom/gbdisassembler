namespace Lag.NESLib.Port
{
    public class STF1 : AbstractPort
    {
        public STF1(Nes dis) : base(dis, 0x400A, nameof(STF1)) { }
    }
}
