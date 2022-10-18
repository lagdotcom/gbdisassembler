namespace Lag.GBLib.Port
{
    public class NR51 : AbstractPort
    {
        public NR51(Gameboy dis) : base(dis, 0xFF25, nameof(NR51)) { }
    }
}
