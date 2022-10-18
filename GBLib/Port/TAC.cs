namespace Lag.GBLib.Port
{
    public class TAC : AbstractPort
    {
        public TAC(Gameboy dis) : base(dis, 0xFF07, nameof(TAC)) { }
    }
}
