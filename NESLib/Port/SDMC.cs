namespace Lag.NESLib.Port
{
    public class SDMC : AbstractPort
    {
        public SDMC(Nes dis) : base(dis, 0x4010, nameof(SDMC)) { }
    }
}
