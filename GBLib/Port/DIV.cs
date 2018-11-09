namespace GBLib.Port
{
    public class DIV : AbstractPort
    {
        public DIV(Disassembler dis) : base(dis, 0xFF04, nameof(DIV)) { }
    }
}
