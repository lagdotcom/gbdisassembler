namespace Lag.GBLib.Port
{
    public class SCX : AbstractPort
    {
        public SCX(Gameboy dis) : base(dis, 0xFF43, nameof(SCX)) { }
    }
}
