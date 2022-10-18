namespace Lag.NESLib.Port
{
    public class PPUS : AbstractPort
    {
        public PPUS(Nes dis) : base(dis, 0x2002, nameof(PPUS)) { }
    }
}
