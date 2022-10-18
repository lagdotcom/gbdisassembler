namespace Lag.GBLib.Port
{
    public class HDMA1 : AbstractPort
    {
        public HDMA1(Gameboy dis) : base(dis, 0xFF51, nameof(HDMA1)) { }
    }
}
