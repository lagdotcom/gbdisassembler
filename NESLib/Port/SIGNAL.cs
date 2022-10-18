namespace Lag.NESLib.Port
{
    public class SIGNAL : AbstractPort
    {
        public SIGNAL(Nes dis) : base(dis, 0x4015, nameof(SIGNAL)) { }
    }
}
