namespace GBLib.Port
{
    public class SC : AbstractPort
    {
        public SC(Disassembler dis) : base(dis, 0xFF02, nameof(SC)) { }
    }
}
