namespace GBLib.Port
{
    public class OBPI : AbstractPort
    {
        public OBPI(Disassembler dis) : base(dis, 0xFF6A, nameof(OBPI)) { }
    }
}
