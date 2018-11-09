namespace GBLib.Port
{
    public class NR23 : AbstractPort
    {
        public NR23(Disassembler dis) : base(dis, 0xFF18, nameof(NR23)) { }
    }
}
