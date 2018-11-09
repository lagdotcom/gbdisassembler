namespace GBLib.Port
{
    public class LCDC : AbstractPort
    {
        public LCDC(Disassembler dis) : base(dis, 0xFF40, nameof(LCDC)) { }
    }
}
