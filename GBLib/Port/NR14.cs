namespace Lag.GBLib.Port
{
    public class NR14 : AbstractPort
    {
        public NR14(Gameboy dis) : base(dis, 0xFF14, nameof(NR14)) { }
    }
}
