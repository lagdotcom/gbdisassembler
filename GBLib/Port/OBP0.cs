namespace GBLib.Port
{
    public class OBP0 : AbstractPort
    {
        public OBP0(Disassembler dis) : base(dis, 0xFF48, nameof(OBP0)) { }
    }
}
