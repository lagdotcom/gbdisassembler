namespace GBLib.Port
{
    public class SB : AbstractPort
    {
        public SB(Disassembler dis) : base(dis, 0xFF01, nameof(SB)) { }
    }
}
