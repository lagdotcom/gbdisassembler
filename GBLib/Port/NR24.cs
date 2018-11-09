namespace GBLib.Port
{
    public class NR24 : AbstractPort
    {
        public NR24(Disassembler dis) : base(dis, 0xFF19, nameof(NR24)) { }
    }
}
