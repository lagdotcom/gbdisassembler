namespace Lag.NESLib.Port
{
    public class STC1 : AbstractPort
    {
        public STC1(Nes dis) : base(dis, 0x4008, nameof(STC1)) { }
    }
}
