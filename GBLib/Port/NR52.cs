namespace GBLib.Port
{
    public class NR52 : AbstractPort
    {
        public NR52(Disassembler dis) : base(dis, 0xFF26, nameof(NR52)) { }
    }
}
