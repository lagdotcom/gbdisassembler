namespace GBLib.Port
{
    public class HDMA2 : AbstractPort
    {
        public HDMA2(Disassembler dis) : base(dis, 0xFF52, nameof(HDMA2)) { }
    }
}
