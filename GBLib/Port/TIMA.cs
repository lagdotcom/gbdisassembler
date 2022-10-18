namespace Lag.GBLib.Port
{
    public class TIMA : AbstractPort
    {
        public TIMA(Gameboy dis) : base(dis, 0xFF05, nameof(TIMA)) { }
    }
}
