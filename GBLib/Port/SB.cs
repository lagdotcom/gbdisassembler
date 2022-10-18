namespace Lag.GBLib.Port
{
    public class SB : AbstractPort
    {
        public SB(Gameboy dis) : base(dis, 0xFF01, nameof(SB)) { }
    }
}
