namespace Lag.GBLib.Port
{
    public class BGPD : AbstractPort
    {
        public BGPD(Gameboy dis) : base(dis, 0xFF69, nameof(BGPD)) { }
    }
}
