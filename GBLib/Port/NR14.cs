namespace GBLib.Port
{
    public class NR14 : AbstractPort
    {
        public NR14(Disassembler dis) : base(dis, 0xFF14, nameof(NR14)) { }
    }
}
