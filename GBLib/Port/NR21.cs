namespace Lag.GBLib.Port
{
    public class NR21 : AbstractPort
    {
        public NR21(Gameboy dis) : base(dis, 0xFF16, nameof(NR21)) { }
    }
}
