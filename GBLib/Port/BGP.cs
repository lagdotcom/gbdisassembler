namespace Lag.GBLib.Port
{
    public class BGP : AbstractPort
    {
        public BGP(Gameboy dis) : base(dis, 0xFF47, nameof(BGP)) { }
    }
}
