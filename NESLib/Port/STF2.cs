namespace Lag.NESLib.Port
{
    public class STF2 : AbstractPort
    {
        public STF2(Nes dis) : base(dis, 0x400B, nameof(STF2)) { }
    }
}
