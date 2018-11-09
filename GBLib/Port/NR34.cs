namespace GBLib.Port
{
    public class NR34 : AbstractPort
    {
        public NR34(Disassembler dis) : base(dis, 0xFF1E, nameof(NR34)) { }
    }
}
