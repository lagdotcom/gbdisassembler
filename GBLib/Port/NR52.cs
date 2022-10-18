namespace Lag.GBLib.Port
{
    public class NR52 : AbstractPort
    {
        public NR52(Gameboy dis) : base(dis, 0xFF26, nameof(NR52)) { }
    }
}
