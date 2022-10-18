namespace Lag.GBLib.Port
{
    public class NR23 : AbstractPort
    {
        public NR23(Gameboy dis) : base(dis, 0xFF18, nameof(NR23)) { }
    }
}
