namespace Lag.GBLib.Port
{
    public class NR44 : AbstractPort
    {
        public NR44(Gameboy dis) : base(dis, 0xFF23, nameof(NR44)) { }
    }
}
