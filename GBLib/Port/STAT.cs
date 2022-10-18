namespace Lag.GBLib.Port
{
    public class STAT : AbstractPort
    {
        public STAT(Gameboy dis) : base(dis, 0xFF41, nameof(STAT)) { }
    }
}
