namespace Lag.GBLib.Port
{
    public class NR24 : AbstractPort
    {
        public NR24(Gameboy dis) : base(dis, 0xFF19, nameof(NR24)) { }
    }
}
