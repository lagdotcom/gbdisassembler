namespace GBLib.Port
{
    public class NR11 : AbstractPort
    {
        public NR11(Disassembler dis) : base(dis, 0xFF11, nameof(NR11)) { }
    }
}
