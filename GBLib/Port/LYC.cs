namespace GBLib.Port
{
    public class LYC : AbstractPort
    {
        public LYC(Disassembler dis) : base(dis, 0xFF45, nameof(LYC)) { }
    }
}
