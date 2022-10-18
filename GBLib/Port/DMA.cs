namespace Lag.GBLib.Port
{
    public class DMA : AbstractPort
    {
        public DMA(Gameboy dis) : base(dis, 0xFF46, nameof(DMA)) { }
    }
}
