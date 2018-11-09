namespace GBLib.Port
{
    public class NR44 : AbstractPort
    {
        public NR44(Disassembler dis) : base(dis, 0xFF23, nameof(NR44)) { }
    }
}
