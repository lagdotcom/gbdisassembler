namespace Lag.GBLib.Port
{
    public class NR10 : AbstractPort
    {
        public NR10(Gameboy dis) : base(dis, 0xFF10, nameof(NR10)) { }
    }
}
