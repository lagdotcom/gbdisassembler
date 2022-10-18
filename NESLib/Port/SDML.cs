namespace Lag.NESLib.Port
{
    public class SDML : AbstractPort
    {
        public SDML(Nes dis) : base(dis, 0x4013, nameof(SDML)) { }
    }
}
