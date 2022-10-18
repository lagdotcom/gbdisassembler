namespace Lag.GBLib.Port
{
    public class WY : AbstractPort
    {
        public WY(Gameboy dis) : base(dis, 0xFF4A, nameof(WY)) { }
    }
}
