namespace Lag.NESLib.Port
{
    public class STC2 : AbstractPort
    {
        public STC2(Nes dis) : base(dis, 0x4009, nameof(STC2)) { }
    }
}
