namespace Lag.NESLib.Port
{
    public class SNF1 : AbstractPort
    {
        public SNF1(Nes dis) : base(dis, 0x400E, nameof(SNF1)) { }
    }
}
