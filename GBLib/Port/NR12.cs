namespace Lag.GBLib.Port
{
    public class NR12 : AbstractPort
    {
        public NR12(Gameboy dis) : base(dis, 0xFF12, nameof(NR12)) { }
    }
}
