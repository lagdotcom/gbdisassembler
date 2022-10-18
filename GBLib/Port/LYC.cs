namespace Lag.GBLib.Port
{
    public class LYC : AbstractPort
    {
        public LYC(Gameboy dis) : base(dis, 0xFF45, nameof(LYC)) { }
    }
}
