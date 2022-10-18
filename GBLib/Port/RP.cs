namespace Lag.GBLib.Port
{
    public class RP : AbstractPort
    {
        public RP(Gameboy dis) : base(dis, 0xFF56, nameof(RP)) { }
    }
}
