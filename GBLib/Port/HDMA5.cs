namespace GBLib.Port
{
    public class HDMA5 : AbstractPort
    {
        public HDMA5(Disassembler dis) : base(dis, 0xFF55, nameof(HDMA5)) { }
    }
}
