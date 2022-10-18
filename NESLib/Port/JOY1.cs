namespace Lag.NESLib.Port
{
    public class JOY1 : AbstractPort
    {
        public JOY1(Nes dis) : base(dis, 0x4016, nameof(JOY1)) { }
    }
}
