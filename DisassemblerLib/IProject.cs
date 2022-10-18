using System.Collections.Generic;

namespace Lag.DisassemblerLib
{
    public interface IProject
    {
        Dictionary<uint, string> Comments { get; }
        Dictionary<uint, Word> CustomOperands { get; }
        Dictionary<uint, IInstruction> Instructions { get; }
        Dictionary<uint, InstructionContext> Contexts { get; }
        Labeller Labeller { get; }
        string Marker { get; }
        Namer Namer { get; }
        IEnumerable<Segment> Segments { get; }
        IEnumerable<Segment> RAMSegments { get; }

        byte[] ROM { get; }
        uint ROMSize { get; }
        int MaxOpSize { get; }
        string Filename { get; set; }

        void AcquireROM();
        void AddCustomOperand(uint location, Word operand);
        void Analyse(uint start, bool force = false);
        void DeleteCustomOperand(uint address);
        void Export(string fileName);
        IPortHandler FindHandler(Word address);
        Word GuessFromContext(uint context, uint value);
        IInstruction Parse(uint address);
        void StandardAnalysis();
    }
}
