namespace GBLib.Port
{
    public class OBPD : AbstractPort
    {
        public OBPD(Disassembler dis) : base(dis, 0xFF6B, nameof(OBPD)) { }
    }
}
