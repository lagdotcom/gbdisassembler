namespace Lag.GBLib.Port
{
    public class NR32 : AbstractPort
    {
        public NR32(Gameboy dis) : base(dis, 0xFF1C, nameof(NR32)) { }
    }
}
