using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Lag.NESLib
{
    public class Instruction : IInstruction
    {
        public Instruction() { }

        public Instruction(Nes project, OpCode op, uint loc) : this()
        {
            Project = project;
            Location = loc;
            Address = project.FromAbsolute(loc);
            Op = op;
            OperandSize = RP2A03.GetOperandSize(op);

            OperandBytes = Project.ROM.Slice(loc + 1, loc + TotalSize);
            IsEnd = RP2A03.IsOpEnd(op);
            IsReal = true;

            OpType = DetermineOpType(Op);
            Operands = DetermineOperands();
            JumpLocation = RP2A03.GetJumpDestination(this);
            for (int i = 0; i < Operands.Length; i++)
            {
                if (Operands[i] is Word)
                {
                    WordOperandIndex = i;
                    break;
                }
            }
        }

        public static Instruction DataWord(Nes project, Word op, uint loc) => new Instruction()
        {
            Project = project,
            Location = loc,
            Address = project.FromAbsolute(loc),
            Op = OpCode.DATA_WORD,
            OperandSize = 1,    // I know this looks weird, it's because of TotalSize
            IsEnd = false,
            IsReal = false,
            OpType = "dw",
            Operands = new IOperand[] { op },
            WordOperandIndex = 0
        };

        private IOperand[] DetermineOperands()
        {
            OpBuilder ops = new OpBuilder(this, OperandBytes);

            switch (Op)
            {
                case OpCode.ADC_abs:
                case OpCode.AND_abs:
                case OpCode.ASL_abs:
                case OpCode.BIT_abs:
                case OpCode.CMP_abs:
                case OpCode.CPX_abs:
                case OpCode.CPY_abs:
                case OpCode.EOR_abs:
                case OpCode.JMP_abs:
                case OpCode.JSR_abs:
                case OpCode.LSR_abs:
                case OpCode.ORA_abs:
                case OpCode.ROL_abs:
                case OpCode.ROR_abs:
                case OpCode.SBC_abs:
                    return ops.Absolute().Resolve();

                case OpCode.LDA_abs:
                case OpCode.LDX_abs:
                case OpCode.LDY_abs:
                    return ops.Absolute().IsRead().Resolve();

                case OpCode.DEC_abs:
                case OpCode.INC_abs:
                case OpCode.STA_abs:
                case OpCode.STX_abs:
                case OpCode.STY_abs:
                    return ops.Absolute().IsWrite().Resolve();

                case OpCode.ADC_abs_X:
                case OpCode.AND_abs_X:
                case OpCode.ASL_abs_X:
                case OpCode.CMP_abs_X:
                case OpCode.DEC_abs_X:
                case OpCode.EOR_abs_X:
                case OpCode.INC_abs_X:
                case OpCode.LDA_abs_X:
                case OpCode.LDY_abs_X:
                case OpCode.LSR_abs_X:
                case OpCode.ORA_abs_X:
                case OpCode.ROL_abs_X:
                case OpCode.ROR_abs_X:
                case OpCode.SBC_abs_X:
                case OpCode.STA_abs_X:
                    return ops.Absolute().Index("X").Resolve();

                case OpCode.ADC_abs_Y:
                case OpCode.AND_abs_Y:
                case OpCode.CMP_abs_Y:
                case OpCode.EOR_abs_Y:
                case OpCode.LDA_abs_Y:
                case OpCode.LDX_abs_Y:
                case OpCode.ORA_abs_Y:
                case OpCode.SBC_abs_Y:
                case OpCode.STA_abs_Y:
                    return ops.Absolute().Index("Y").Resolve();

                case OpCode.ADC_imm:
                case OpCode.AND_imm:
                case OpCode.CMP_imm:
                case OpCode.CPX_imm:
                case OpCode.CPY_imm:
                case OpCode.EOR_imm:
                case OpCode.LDA_imm:
                case OpCode.LDX_imm:
                case OpCode.LDY_imm:
                case OpCode.ORA_imm:
                case OpCode.SBC_imm:
                    return ops.Immediate().Resolve();

                case OpCode.ADC_ind_X:
                case OpCode.AND_ind_X:
                case OpCode.CMP_ind_X:
                case OpCode.EOR_ind_X:
                case OpCode.LDA_ind_X:
                case OpCode.ORA_ind_X:
                case OpCode.SBC_ind_X:
                case OpCode.STA_ind_X:
                    return ops.Indirect("X").Resolve();

                case OpCode.ADC_ind_Y:
                case OpCode.AND_ind_Y:
                case OpCode.CMP_ind_Y:
                case OpCode.EOR_ind_Y:
                case OpCode.LDA_ind_Y:
                case OpCode.ORA_ind_Y:
                case OpCode.SBC_ind_Y:
                case OpCode.STA_ind_Y:
                    return ops.Indirect().Index("Y").Resolve();

                case OpCode.BCC_rel:
                case OpCode.BCS_rel:
                case OpCode.BEQ_rel:
                case OpCode.BMI_rel:
                case OpCode.BNE_rel:
                case OpCode.BPL_rel:
                case OpCode.BVC_rel:
                case OpCode.BVS_rel:
                    return ops.Relative().Resolve();

                case OpCode.ADC_zpg:
                case OpCode.AND_zpg:
                case OpCode.ASL_zpg:
                case OpCode.BIT_zpg:
                case OpCode.CMP_zpg:
                case OpCode.CPX_zpg:
                case OpCode.CPY_zpg:
                case OpCode.DEC_zpg:
                case OpCode.EOR_zpg:
                case OpCode.INC_zpg:
                case OpCode.LDA_zpg:
                case OpCode.LDX_zpg:
                case OpCode.LDY_zpg:
                case OpCode.LSR_zpg:
                case OpCode.ORA_zpg:
                case OpCode.ROL_zpg:
                case OpCode.ROR_zpg:
                case OpCode.SBC_zpg:
                case OpCode.STA_zpg:
                case OpCode.STX_zpg:
                case OpCode.STY_zpg:
                    return ops.ZeroPage().Resolve();

                case OpCode.ADC_zpg_X:
                case OpCode.AND_zpg_X:
                case OpCode.ASL_zpg_X:
                case OpCode.CMP_zpg_X:
                case OpCode.DEC_zpg_X:
                case OpCode.EOR_zpg_X:
                case OpCode.INC_zpg_X:
                case OpCode.LDA_zpg_X:
                case OpCode.LDY_zpg_X:
                case OpCode.LSR_zpg_X:
                case OpCode.ORA_zpg_X:
                case OpCode.ROL_zpg_X:
                case OpCode.ROR_zpg_X:
                case OpCode.SBC_zpg_X:
                case OpCode.STA_zpg_X:
                case OpCode.STY_zpg_X:
                    return ops.ZeroPage().Index("X").Resolve();

                case OpCode.LDX_zpg_Y:
                case OpCode.STX_zpg_Y:
                    return ops.ZeroPage().Index("Y").Resolve();

                case OpCode.ASL_A:
                case OpCode.LSR_A:
                case OpCode.ROL_A:
                case OpCode.ROR_A:
                    return ops.Register("A").Resolve();

                default: return Array.Empty<IOperand>();
            }
        }

        private string DetermineOpType(OpCode op)
        {
            switch (op)
            {
                case OpCode.ADC_abs:
                case OpCode.ADC_abs_X:
                case OpCode.ADC_abs_Y:
                case OpCode.ADC_imm:
                case OpCode.ADC_ind_X:
                case OpCode.ADC_ind_Y:
                case OpCode.ADC_zpg:
                case OpCode.ADC_zpg_X:
                    return "ADC";

                case OpCode.AND_abs:
                case OpCode.AND_abs_X:
                case OpCode.AND_abs_Y:
                case OpCode.AND_imm:
                case OpCode.AND_ind_X:
                case OpCode.AND_ind_Y:
                case OpCode.AND_zpg:
                case OpCode.AND_zpg_X:
                    return "AND";

                case OpCode.ASL_A:
                case OpCode.ASL_abs:
                case OpCode.ASL_abs_X:
                case OpCode.ASL_zpg:
                case OpCode.ASL_zpg_X:
                    return "ASL";

                case OpCode.BCC_rel: return "BCC";
                case OpCode.BCS_rel: return "BCS";
                case OpCode.BEQ_rel: return "BEQ";

                case OpCode.BIT_abs:
                case OpCode.BIT_zpg:
                    return "BIT";

                case OpCode.BMI_rel: return "BMI";
                case OpCode.BNE_rel: return "BNE";
                case OpCode.BPL_rel: return "BPL";
                case OpCode.BRK: return "BRK";
                case OpCode.BVC_rel: return "BVC";
                case OpCode.BVS_rel: return "BVS";
                case OpCode.CLC: return "CLC";
                case OpCode.CLD: return "CLD";
                case OpCode.CLI: return "CLI";
                case OpCode.CLV: return "CLV";

                case OpCode.CMP_abs:
                case OpCode.CMP_abs_X:
                case OpCode.CMP_abs_Y:
                case OpCode.CMP_imm:
                case OpCode.CMP_ind_X:
                case OpCode.CMP_ind_Y:
                case OpCode.CMP_zpg:
                case OpCode.CMP_zpg_X:
                    return "CMP";

                case OpCode.CPX_abs:
                case OpCode.CPX_imm:
                case OpCode.CPX_zpg:
                    return "CPX";

                case OpCode.CPY_abs:
                case OpCode.CPY_imm:
                case OpCode.CPY_zpg:
                    return "CPY";

                case OpCode.DEC_abs:
                case OpCode.DEC_abs_X:
                case OpCode.DEC_zpg:
                case OpCode.DEC_zpg_X:
                    return "DEC";

                case OpCode.DEX: return "DEX";
                case OpCode.DEY: return "DEY";

                case OpCode.EOR_abs:
                case OpCode.EOR_abs_X:
                case OpCode.EOR_abs_Y:
                case OpCode.EOR_imm:
                case OpCode.EOR_ind_X:
                case OpCode.EOR_ind_Y:
                case OpCode.EOR_zpg:
                case OpCode.EOR_zpg_X:
                    return "EOR";

                case OpCode.INC_abs:
                case OpCode.INC_abs_X:
                case OpCode.INC_zpg:
                case OpCode.INC_zpg_X:
                    return "INC";

                case OpCode.INX: return "INX";
                case OpCode.INY: return "INY";

                case OpCode.JMP_abs:
                case OpCode.JMP_ind:
                    return "JMP";

                case OpCode.JSR_abs: return "JSR";

                case OpCode.LDA_abs:
                case OpCode.LDA_abs_X:
                case OpCode.LDA_abs_Y:
                case OpCode.LDA_imm:
                case OpCode.LDA_ind_X:
                case OpCode.LDA_ind_Y:
                case OpCode.LDA_zpg:
                case OpCode.LDA_zpg_X:
                    return "LDA";

                case OpCode.LDX_abs:
                case OpCode.LDX_abs_Y:
                case OpCode.LDX_imm:
                case OpCode.LDX_zpg:
                case OpCode.LDX_zpg_Y:
                    return "LDX";

                case OpCode.LDY_abs:
                case OpCode.LDY_abs_X:
                case OpCode.LDY_imm:
                case OpCode.LDY_zpg:
                case OpCode.LDY_zpg_X:
                    return "LDY";

                case OpCode.LSR_A:
                case OpCode.LSR_abs:
                case OpCode.LSR_abs_X:
                case OpCode.LSR_zpg:
                case OpCode.LSR_zpg_X:
                    return "LSR";

                case OpCode.NOP: return "NOP";

                case OpCode.ORA_abs:
                case OpCode.ORA_abs_X:
                case OpCode.ORA_abs_Y:
                case OpCode.ORA_imm:
                case OpCode.ORA_ind_X:
                case OpCode.ORA_ind_Y:
                case OpCode.ORA_zpg:
                case OpCode.ORA_zpg_X:
                    return "ORA";

                case OpCode.PHA: return "PHA";
                case OpCode.PHP: return "PHP";
                case OpCode.PLA: return "PLA";
                case OpCode.PLP: return "PLP";

                case OpCode.ROL_A:
                case OpCode.ROL_abs:
                case OpCode.ROL_abs_X:
                case OpCode.ROL_zpg:
                case OpCode.ROL_zpg_X:
                    return "ROL";

                case OpCode.ROR_A:
                case OpCode.ROR_abs:
                case OpCode.ROR_abs_X:
                case OpCode.ROR_zpg:
                case OpCode.ROR_zpg_X:
                    return "ROR";

                case OpCode.RTI: return "RTI";
                case OpCode.RTS: return "RTS";

                case OpCode.SBC_abs:
                case OpCode.SBC_abs_X:
                case OpCode.SBC_abs_Y:
                case OpCode.SBC_imm:
                case OpCode.SBC_ind_X:
                case OpCode.SBC_ind_Y:
                case OpCode.SBC_zpg:
                case OpCode.SBC_zpg_X:
                    return "SBC";

                case OpCode.SEC: return "SEC";
                case OpCode.SED: return "SED";
                case OpCode.SEI: return "SEI";

                case OpCode.STA_abs:
                case OpCode.STA_abs_X:
                case OpCode.STA_abs_Y:
                case OpCode.STA_ind_X:
                case OpCode.STA_ind_Y:
                case OpCode.STA_zpg:
                case OpCode.STA_zpg_X:
                    return "STA";

                case OpCode.STX_abs:
                case OpCode.STX_zpg:
                case OpCode.STX_zpg_Y:
                    return "STX";

                case OpCode.STY_abs:
                case OpCode.STY_zpg:
                case OpCode.STY_zpg_X:
                    return "STY";

                case OpCode.TAX: return "TAX";
                case OpCode.TAY: return "TAY";
                case OpCode.TSX: return "TSX";
                case OpCode.TXA: return "TXA";
                case OpCode.TXS: return "TXS";
                case OpCode.TYA: return "TYA";

                default: return "?";
            }
        }

        public Word Address { get; private set; }
        public uint? JumpLocation { get; }
        public Nes Project { get; private set; }
        public OpCode Op { get; private set; }
        public byte[] OperandBytes { get; }
        public uint OperandSize { get; private set; }

        #region IInstruction
        public bool IsEnd { get; private set; }

        public bool IsReal { get; private set; }

        public uint Location { get; private set; }

        public IOperand[] Operands { get; private set; }

        public string OpType { get; private set; }

        public uint TotalSize => 1 + OperandSize;

        public int? WordOperandIndex { get; private set; }
        #endregion

        public override string ToString() => $"{Address} | {OpType} {string.Join(",", OperandStrings)}".Trim();

        public IEnumerable<string> OperandStrings => Operands != null ? Operands.Select(op => ImproveOperand(op)) : Array.Empty<string>();

        private string ImproveOperand(IOperand op)
        {
            if (op is Word word)
            {
                IPortHandler handler = Project.FindHandler(word);
                if (handler != null)
                    return handler.Identify(word);

                return word.ToString();
            }

            return op.ToString();
        }

        public void WriteAsm(StreamWriter sw)
        {
            throw new NotImplementedException();
        }
    }
}
