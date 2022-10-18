namespace Lag.GBLib.Port
{
    public class WX : AbstractPort
    {
        public WX(Gameboy dis) : base(dis, 0xFF4B, nameof(WX)) { }
    }
}
