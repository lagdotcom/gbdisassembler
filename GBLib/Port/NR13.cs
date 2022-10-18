namespace Lag.GBLib.Port
{
    public class NR13 : AbstractPort
    {
        public NR13(Gameboy dis) : base(dis, 0xFF13, nameof(NR13)) { }
    }
}
