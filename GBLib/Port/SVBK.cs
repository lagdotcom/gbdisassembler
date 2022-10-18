namespace Lag.GBLib.Port
{
    public class SVBK : AbstractPort
    {
        public SVBK(Gameboy dis) : base(dis, 0xFF70, nameof(SVBK)) { }
    }
}
