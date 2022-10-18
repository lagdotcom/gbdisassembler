namespace Lag.GBLib.Port
{
    public class NR30 : AbstractPort
    {
        public NR30(Gameboy dis) : base(dis, 0xFF1A, nameof(NR30)) { }
    }
}
