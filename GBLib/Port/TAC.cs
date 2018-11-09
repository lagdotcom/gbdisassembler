namespace GBLib.Port
{
    public class TAC : AbstractPort
    {
        public TAC(Disassembler dis) : base(dis, 0xFF07, nameof(TAC)) { }
    }
}
