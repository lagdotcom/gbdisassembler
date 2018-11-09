namespace GBLib.Port
{
    public class LY : AbstractPort
    {
        public LY(Disassembler dis) : base(dis, 0xFF44, nameof(LY)) { }
    }
}
