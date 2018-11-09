namespace GBLib.Port
{
    public class BGPI : AbstractPort
    {
        public BGPI(Disassembler dis) : base(dis, 0xFF68, nameof(BGPI)) { }
    }
}
