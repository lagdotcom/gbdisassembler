namespace GBLib.Port
{
    public class DMA : AbstractPort
    {
        public DMA(Disassembler dis) : base(dis, 0xFF46, nameof(DMA)) { }
    }
}
