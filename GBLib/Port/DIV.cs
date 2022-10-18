namespace Lag.GBLib.Port
{
    public class DIV : AbstractPort
    {
        public DIV(Gameboy dis) : base(dis, 0xFF04, nameof(DIV)) { }
    }
}
