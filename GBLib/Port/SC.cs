namespace Lag.GBLib.Port
{
    public class SC : AbstractPort
    {
        public SC(Gameboy dis) : base(dis, 0xFF02, nameof(SC)) { }
    }
}
