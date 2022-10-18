namespace Lag.GBLib.Port
{
    public class OBPI : AbstractPort
    {
        public OBPI(Gameboy dis) : base(dis, 0xFF6A, nameof(OBPI)) { }
    }
}
