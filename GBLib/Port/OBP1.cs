namespace Lag.GBLib.Port
{
    public class OBP1 : AbstractPort
    {
        public OBP1(Gameboy dis) : base(dis, 0xFF49, nameof(OBP1)) { }
    }
}
