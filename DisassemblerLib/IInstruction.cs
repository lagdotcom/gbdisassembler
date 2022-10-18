namespace Lag.DisassemblerLib
{
    public interface IInstruction
    {
        bool IsEnd { get; }
        bool IsReal { get; }
        uint Location { get; }
        IOperand[] Operands { get; }
        string OpType { get; }
        uint TotalSize { get; }
        int? WordOperandIndex { get; }

        void WriteAsm(System.IO.StreamWriter sw);
    }
}
