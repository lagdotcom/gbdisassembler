namespace Lag.GBLib.Port
{
    public class NR31 : AbstractPort
    {
        public NR31(Gameboy dis) : base(dis, 0xFF1B, nameof(NR31)) { }
    }
}
