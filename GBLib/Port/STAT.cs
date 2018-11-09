namespace GBLib.Port
{
    public class STAT : AbstractPort
    {
        public STAT(Disassembler dis) : base(dis, 0xFF41, nameof(STAT)) { }
    }
}
