namespace GBLib.Port
{
    public class BGP : AbstractPort
    {
        public BGP(Disassembler dis) : base(dis, 0xFF47, nameof(BGP)) { }
    }
}
