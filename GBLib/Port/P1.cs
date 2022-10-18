namespace Lag.GBLib.Port
{
    public class P1 : AbstractPort
    {
        public P1(Gameboy dis) : base(dis, 0xFF00, nameof(P1)) { }
    }
}
