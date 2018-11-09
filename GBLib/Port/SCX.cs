namespace GBLib.Port
{
    public class SCX : AbstractPort
    {
        public SCX(Disassembler dis) : base(dis, 0xFF43, nameof(SCX)) { }
    }
}
