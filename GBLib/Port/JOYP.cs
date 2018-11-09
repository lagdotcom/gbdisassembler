namespace GBLib.Port
{
    public class JOYP : AbstractPort
    {
        public JOYP(Disassembler dis) : base(dis, 0xFF00, nameof(JOYP)) { }
    }
}
