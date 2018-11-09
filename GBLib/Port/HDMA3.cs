namespace GBLib.Port
{
    public class HDMA3 : AbstractPort
    {
        public HDMA3(Disassembler dis) : base(dis, 0xFF53, nameof(HDMA3)) { }
    }
}
