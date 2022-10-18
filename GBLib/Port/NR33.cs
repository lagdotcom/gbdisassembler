namespace Lag.GBLib.Port
{
    public class NR33 : AbstractPort
    {
        public NR33(Gameboy dis) : base(dis, 0xFF1D, nameof(NR33)) { }
    }
}
