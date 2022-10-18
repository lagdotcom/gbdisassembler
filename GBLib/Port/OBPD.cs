namespace Lag.GBLib.Port
{
    public class OBPD : AbstractPort
    {
        public OBPD(Gameboy dis) : base(dis, 0xFF6B, nameof(OBPD)) { }
    }
}
