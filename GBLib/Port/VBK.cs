namespace Lag.GBLib.Port
{
    public class VBK : AbstractPort
    {
        public VBK(Gameboy dis) : base(dis, 0xFF4F, nameof(VBK)) { }
    }
}
