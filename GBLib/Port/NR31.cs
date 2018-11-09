namespace GBLib.Port
{
    public class NR31 : AbstractPort
    {
        public NR31(Disassembler dis) : base(dis, 0xFF1B, nameof(NR31)) { }
    }
}
