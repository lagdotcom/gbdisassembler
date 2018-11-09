namespace GBLib.Port
{
    public class NR41 : AbstractPort
    {
        public NR41(Disassembler dis) : base(dis, 0xFF20, nameof(NR41)) { }
    }
}
