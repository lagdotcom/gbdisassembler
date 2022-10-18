namespace Lag.GBLib.Port
{
    public class OBP0 : AbstractPort
    {
        public OBP0(Gameboy dis) : base(dis, 0xFF48, nameof(OBP0)) { }
    }
}
