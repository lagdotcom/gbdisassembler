namespace GBLib.Port
{
    public class RP : AbstractPort
    {
        public RP(Disassembler dis) : base(dis, 0xFF56, nameof(RP)) { }
    }
}
