namespace GBLib.Port
{
    public class NR10 : AbstractPort
    {
        public NR10(Disassembler dis) : base(dis, 0xFF10, nameof(NR10)) { }
    }
}
