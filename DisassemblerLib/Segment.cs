namespace Lag.DisassemblerLib
{
    public class Segment
    {
        public Segment(string name, uint mount, uint length, uint offset)
        {
            Name = name;
            RAMPosition = mount;
            Length = length;
            FileOffset = offset;
        }

        public string Name { get; set; }
        public uint RAMPosition { get; set; }
        public uint Length { get; set; }
        public uint FileOffset { get; set; }
        public uint End => FileOffset + Length;
        public uint RAMEnd => RAMPosition + Length;

        public bool Contains(uint location) => location >= FileOffset && location < End;

        public bool RAMContains(uint location) => location >= RAMPosition && location < RAMEnd;

        public override string ToString() => $"{Name}@{RAMPosition:X4}+{Length:X4}";

        public override int GetHashCode() => Name.GetHashCode();

        public override bool Equals(object obj) => (obj as Segment)?.Name == Name;
    }
}
