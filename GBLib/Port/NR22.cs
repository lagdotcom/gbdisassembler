namespace Lag.GBLib.Port
{
    public class NR22 : AbstractPort
    {
        public NR22(Gameboy dis) : base(dis, 0xFF17, nameof(NR22)) { }
    }
}
