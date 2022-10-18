namespace Lag.NESLib.Port
{
    public class SNF2 : AbstractPort
    {
        public SNF2(Nes dis) : base(dis, 0x400F, nameof(SNF2)) { }
    }
}
