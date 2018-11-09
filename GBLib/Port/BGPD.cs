namespace GBLib.Port
{
    public class BGPD : AbstractPort
    {
        public BGPD(Disassembler dis) : base(dis, 0xFF69, nameof(BGPD)) { }
    }
}
