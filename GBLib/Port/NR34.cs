namespace Lag.GBLib.Port
{
    public class NR34 : AbstractPort
    {
        public NR34(Gameboy dis) : base(dis, 0xFF1E, nameof(NR34)) { }
    }
}
