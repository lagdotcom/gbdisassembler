namespace GBLib.Port
{
    public class NR42 : AbstractPort
    {
        public NR42(Disassembler dis) : base(dis, 0xFF21, nameof(NR42)) { }
    }
}
