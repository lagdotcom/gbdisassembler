namespace GBLib.Port
{
    public class NR43 : AbstractPort
    {
        public NR43(Disassembler dis) : base(dis, 0xFF22, nameof(NR43)) { }
    }
}
