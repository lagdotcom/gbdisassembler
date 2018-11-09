namespace GBLib.Port
{
    public class NR12 : AbstractPort
    {
        public NR12(Disassembler dis) : base(dis, 0xFF12, nameof(NR12)) { }
    }
}
