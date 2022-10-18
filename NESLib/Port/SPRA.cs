namespace Lag.NESLib.Port
{
    public class SPRA : AbstractPort
    {
        public SPRA(Nes dis) : base(dis, 0x2003, nameof(SPRA)) { }
    }
}
