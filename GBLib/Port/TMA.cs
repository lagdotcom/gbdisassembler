namespace GBLib.Port
{
    public class TMA : AbstractPort
    {
        public TMA(Disassembler dis) : base(dis, 0xFF06, nameof(TMA)) { }
    }
}
