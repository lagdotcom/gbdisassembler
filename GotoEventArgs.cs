namespace GBDisassembler
{
    public class GotoEventArgs
    {
        public GotoEventArgs(uint location)
        {
            Location = location;
        }

        public uint Location { get; }
    }
}