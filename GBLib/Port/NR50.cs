namespace Lag.GBLib.Port
{
    public class NR50 : AbstractPort
    {
        public NR50(Gameboy dis) : base(dis, 0xFF24, nameof(NR50)) { }
    }
}
