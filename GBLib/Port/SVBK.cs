namespace GBLib.Port
{
    public class SVBK : AbstractPort
    {
        public SVBK(Disassembler dis) : base(dis, 0xFF70, nameof(SVBK)) { }
    }
}
