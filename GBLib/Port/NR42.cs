namespace Lag.GBLib.Port
{
    public class NR42 : AbstractPort
    {
        public NR42(Gameboy dis) : base(dis, 0xFF21, nameof(NR42)) { }
    }
}
