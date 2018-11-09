namespace GBLib.Port
{
    public class TIMA : AbstractPort
    {
        public TIMA(Disassembler dis) : base(dis, 0xFF05, nameof(TIMA)) { }
    }
}
