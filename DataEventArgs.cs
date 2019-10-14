using GBLib;

namespace GBDisassembler
{
    public class DataEventArgs
    {
        public DataEventArgs(uint location, DataType type)
        {
            Location = location;
            Type = type;
        }

        public uint Location { get; }
        public DataType Type { get; }
    }
}
