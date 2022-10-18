namespace Lag.NESLib.Port
{
    public class SDMA : AbstractPort
    {
        public SDMA(Nes dis) : base(dis, 0x4012, nameof(SDMA)) { }
    }
}
