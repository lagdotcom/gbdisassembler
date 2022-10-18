namespace Lag.GBLib.Port
{
    public class LY : AbstractPort
    {
        public LY(Gameboy dis) : base(dis, 0xFF44, nameof(LY)) { }
    }
}
