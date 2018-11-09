namespace GBLib.Port
{
    public class WX : AbstractPort
    {
        public WX(Disassembler dis) : base(dis, 0xFF4B, nameof(WX)) { }
    }
}
