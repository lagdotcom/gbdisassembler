namespace GBLib.Port
{
    public class HDMA1 : AbstractPort
    {
        public HDMA1(Disassembler dis) : base(dis, 0xFF51, nameof(HDMA1)) { }
    }
}
