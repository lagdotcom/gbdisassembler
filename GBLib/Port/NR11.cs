namespace Lag.GBLib.Port
{
    public class NR11 : AbstractPort
    {
        public NR11(Gameboy dis) : base(dis, 0xFF11, nameof(NR11)) { }
    }
}
