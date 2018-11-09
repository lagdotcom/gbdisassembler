namespace GBLib.Port
{
    public class SCY : AbstractPort
    {
        public SCY(Disassembler dis) : base(dis, 0xFF42, nameof(SCY)) { }
    }
}
