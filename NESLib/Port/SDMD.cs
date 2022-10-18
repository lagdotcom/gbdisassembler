namespace Lag.NESLib.Port
{
    public class SDMD : AbstractPort
    {
        public SDMD(Nes dis) : base(dis, 0x4011, nameof(SDMD)) { }
    }
}
