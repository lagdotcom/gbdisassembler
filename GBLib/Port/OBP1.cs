namespace GBLib.Port
{
    public class OBP1 : AbstractPort
    {
        public OBP1(Disassembler dis) : base(dis, 0xFF49, nameof(OBP1)) { }
    }
}
