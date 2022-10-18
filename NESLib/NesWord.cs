using Lag.DisassemblerLib;

namespace Lag.NESLib
{
    class NesWord : Word
    {
        public NesWord(uint value = 0) : base(value) { }
        public NesWord(Segment seg, uint value) : base(seg, value) { }

        public string Index { get; set; }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Index)) return base.ToString();

            string address = AddressString();
            string indexed = $"{address},{Index}";

            return Indirect ? $"({indexed})" : indexed;
        }
    }
}
