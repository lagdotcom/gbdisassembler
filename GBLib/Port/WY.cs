namespace GBLib.Port
{
    public class WY : AbstractPort
    {
        public WY(Disassembler dis) : base(dis, 0xFF4A, nameof(WY)) { }
    }
}
