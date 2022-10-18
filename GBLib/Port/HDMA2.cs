namespace Lag.GBLib.Port
{
    public class HDMA2 : AbstractPort
    {
        public HDMA2(Gameboy dis) : base(dis, 0xFF52, nameof(HDMA2)) { }
    }
}
