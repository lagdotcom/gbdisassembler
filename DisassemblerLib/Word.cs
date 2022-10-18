namespace Lag.DisassemblerLib
{
    public class Word : IOperand
    {
        public Word(uint value = 0)
        {
            IsHex = true;
            RAM = true;
            Offset = value;
        }

        public Word(Segment s, uint offset = 0)
        {
            IsHex = true;
            ROM = true;
            Seg = s;

            if (Seg != null) Offset = offset % s.Length;
            else Offset = offset;
        }

        public bool RAM { get; set; }
        public bool ROM { get; set; }
        public bool IsHex { get; set; }
        public bool IsNumeric => true;
        public bool IsRegister => false;

        public Segment Seg { get; set; }
        public uint Offset { get; set; }

        public uint Mount => Seg == null ? Offset : Seg.RAMPosition + Offset;
        public uint Absolute => Seg == null ? Offset : Seg.FileOffset + Offset;

        public bool Read { get; set; }
        public bool Write { get; set; }
        public bool Indirect { get; set; }
        public bool IO => Read || Write;

        public override string ToString()
        {
            string address = AddressString();
            return Indirect ? $"({address})" : address;
        }

        protected string AddressString()
        {
            string repr = IsHex ? $"${Mount:X4}" : Mount.ToString();
            return ROM ? $"{SegmentName}:{Mount:X4}" : repr;
        }

        protected string SegmentName => Seg == null ? "??" : Seg.Name;
    }
}
