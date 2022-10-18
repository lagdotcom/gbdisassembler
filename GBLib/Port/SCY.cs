namespace Lag.GBLib.Port
{
    public class SCY : AbstractPort
    {
        public SCY(Gameboy dis) : base(dis, 0xFF42, nameof(SCY)) { }
    }
}
