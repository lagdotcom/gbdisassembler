namespace GBLib.Port
{
    public class NR13 : AbstractPort
    {
        public NR13(Disassembler dis) : base(dis, 0xFF13, nameof(NR13)) { }
    }
}
