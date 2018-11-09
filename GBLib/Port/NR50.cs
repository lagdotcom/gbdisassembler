namespace GBLib.Port
{
    public class NR50 : AbstractPort
    {
        public NR50(Disassembler dis) : base(dis, 0xFF24, nameof(NR50)) { }
    }
}
