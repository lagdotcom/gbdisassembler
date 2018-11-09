namespace GBLib
{
    class CPUReg : IOperand
    {
        public static CPUReg A = new CPUReg("A");
        public static CPUReg B = new CPUReg("B");
        public static CPUReg C = new CPUReg("C");
        public static CPUReg CH = new CPUReg("($FF00+C)");  // TODO
        public static CPUReg D = new CPUReg("D");
        public static CPUReg E = new CPUReg("E");
        public static CPUReg H = new CPUReg("H");
        public static CPUReg L = new CPUReg("L");
        public static CPUReg AF = new CPUReg("AF");
        public static CPUReg BC = new CPUReg("BC");
        public static CPUReg BCI = new CPUReg("(BC)");
        public static CPUReg DE = new CPUReg("DE");
        public static CPUReg DEI = new CPUReg("(DE)");
        public static CPUReg HL = new CPUReg("HL");
        public static CPUReg HLI = new CPUReg("(HL)");
        public static CPUReg HLIM = new CPUReg("(HL-)");
        public static CPUReg HLIP = new CPUReg("(HL+)");
        public static CPUReg SP = new CPUReg("SP");

        private CPUReg(string name)
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
