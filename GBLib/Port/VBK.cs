namespace GBLib.Port
{
    public class VBK : AbstractPort
    {
        public VBK(Disassembler dis) : base(dis, 0xFF4F, nameof(VBK)) { }
    }
}
