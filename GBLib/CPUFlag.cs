using System;

namespace GBLib
{
    class CPUFlag : IOperand
    {
        public static string[] FlagNames = { "c", "z", "nc", "nz" };

        public static CPUFlag C = new CPUFlag("c");
        public static CPUFlag Z = new CPUFlag("z");
        public static CPUFlag NC = new CPUFlag("nc");
        public static CPUFlag NZ = new CPUFlag("nz");

        public CPUFlag() { }
        public CPUFlag(string name) : this()
        {
            Name = name;
        }

        public char TypeKey => 'f';
        public uint? TypeValue => (uint)Array.IndexOf(FlagNames, Name);

        public string Name;
        public uint? AbsoluteAddress => null;
        public uint Value => 0;
        public bool Read => false;
        public bool Write => false;
        public bool IsHex => false;
        public bool IsNumeric => false;
        public bool IsRegister => false;

        public override string ToString() => Name;
    }
}
