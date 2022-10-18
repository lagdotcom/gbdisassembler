namespace Lag.NESLib.Port
{
    public class JOY2 : AbstractPort
    {
        public JOY2(Nes dis) : base(dis, 0x4017, nameof(JOY2)) { }
    }
}
