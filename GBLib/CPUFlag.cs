namespace GBLib
{
    class CPUFlag : IOperand
    {
        public static CPUFlag C = new CPUFlag("c");
        public static CPUFlag Z = new CPUFlag("z");
        public static CPUFlag NC = new CPUFlag("nc");
        public static CPUFlag NZ = new CPUFlag("nz");

        public CPUFlag() { }
        public CPUFlag(string name) : this()
        {
            Name = name;
        }

        public string Name;
        public uint? AbsoluteAddress => null;
        public bool Read => false;
        public bool Write => false;

        public override string ToString() => Name;
    }
}
