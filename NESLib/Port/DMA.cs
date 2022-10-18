namespace Lag.NESLib.Port
{
    public class DMA : AbstractPort
    {
        public DMA(Nes dis) : base(dis, 0x4014, nameof(DMA)) { }
    }
}
