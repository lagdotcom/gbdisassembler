namespace GBLib.Port
{
    public class NR21 : AbstractPort
    {
        public NR21(Disassembler dis) : base(dis, 0xFF16, nameof(NR21)) { }
    }
}
