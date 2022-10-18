namespace Lag.GBLib.Port
{
    public class BGPI : AbstractPort
    {
        public BGPI(Gameboy dis) : base(dis, 0xFF68, nameof(BGPI)) { }
    }
}
