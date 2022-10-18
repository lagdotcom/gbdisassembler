namespace Lag.GBLib.Port
{
    public class HDMA3 : AbstractPort
    {
        public HDMA3(Gameboy dis) : base(dis, 0xFF53, nameof(HDMA3)) { }
    }
}
