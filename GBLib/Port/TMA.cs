namespace Lag.GBLib.Port
{
    public class TMA : AbstractPort
    {
        public TMA(Gameboy dis) : base(dis, 0xFF06, nameof(TMA)) { }
    }
}
