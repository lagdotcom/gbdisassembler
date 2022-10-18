using System;
using System.Collections.Generic;
using System.Text;
using Lag.DisassemblerLib;

namespace Lag.NESLib
{
    public class RP2A03
    {
        public RP2A03() { }
        public RP2A03(Nes parent) : this()
        {
            Parent = parent;
        }

        public Nes Parent { get; }

        public Instruction Decode(uint loc)
        {
            OpCode? op = GetOpCode(Parent.ROM[loc]);
            if (op == null) return null;

            return new Instruction(Parent, op.Value, loc);
        }

        internal static uint GetOperandSize(OpCode op)
        {
            switch (op)
            {
                case OpCode.ADC_abs:
                case OpCode.ADC_abs_X:
                case OpCode.ADC_abs_Y:
                case OpCode.AND_abs:
                case OpCode.AND_abs_X:
                case OpCode.AND_abs_Y:
                case OpCode.ASL_abs:
                case OpCode.ASL_abs_X:
                case OpCode.BIT_abs:
                case OpCode.CMP_abs:
                case OpCode.CMP_abs_X:
                case OpCode.CMP_abs_Y:
                case OpCode.CPX_abs:
                case OpCode.CPY_abs:
                case OpCode.DEC_abs:
                case OpCode.DEC_abs_X:
                case OpCode.EOR_abs:
                case OpCode.EOR_abs_X:
                case OpCode.EOR_abs_Y:
                case OpCode.INC_abs:
                case OpCode.INC_abs_X:
                case OpCode.JMP_abs:
                case OpCode.JSR_abs:
                case OpCode.LDA_abs:
                case OpCode.LDA_abs_X:
                case OpCode.LDA_abs_Y:
                case OpCode.LDX_abs:
                case OpCode.LDX_abs_Y:
                case OpCode.LDY_abs:
                case OpCode.LDY_abs_X:
                case OpCode.LSR_abs:
                case OpCode.LSR_abs_X:
                case OpCode.ORA_abs:
                case OpCode.ORA_abs_X:
                case OpCode.ORA_abs_Y:
                case OpCode.ROL_abs:
                case OpCode.ROL_abs_X:
                case OpCode.ROR_abs:
                case OpCode.ROR_abs_X:
                case OpCode.SBC_abs:
                case OpCode.SBC_abs_X:
                case OpCode.SBC_abs_Y:
                case OpCode.STA_abs:
                case OpCode.STA_abs_X:
                case OpCode.STA_abs_Y:
                case OpCode.STX_abs:
                case OpCode.STY_abs:
                    return 2;

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
                    return 1;

                case OpCode.ADC_ind_X:
                case OpCode.ADC_ind_Y:
                case OpCode.AND_ind_X:
                case OpCode.AND_ind_Y:
                case OpCode.CMP_ind_X:
                case OpCode.CMP_ind_Y:
                case OpCode.EOR_ind_X:
                case OpCode.EOR_ind_Y:
                case OpCode.JMP_ind:
                case OpCode.LDA_ind_X:
                case OpCode.LDA_ind_Y:
                case OpCode.ORA_ind_X:
                case OpCode.ORA_ind_Y:
                case OpCode.SBC_ind_X:
                case OpCode.SBC_ind_Y:
                case OpCode.STA_ind_X:
                case OpCode.STA_ind_Y:
                    return 2;

                case OpCode.BCC_rel:
                case OpCode.BCS_rel:
                case OpCode.BEQ_rel:
                case OpCode.BMI_rel:
                case OpCode.BNE_rel:
                case OpCode.BPL_rel:
                case OpCode.BVC_rel:
                case OpCode.BVS_rel:
                    return 1;

                case OpCode.ADC_zpg:
                case OpCode.ADC_zpg_X:
                case OpCode.AND_zpg:
                case OpCode.AND_zpg_X:
                case OpCode.ASL_zpg:
                case OpCode.ASL_zpg_X:
                case OpCode.BIT_zpg:
                case OpCode.CMP_zpg:
                case OpCode.CMP_zpg_X:
                case OpCode.CPX_zpg:
                case OpCode.CPY_zpg:
                case OpCode.DEC_zpg:
                case OpCode.DEC_zpg_X:
                case OpCode.EOR_zpg:
                case OpCode.EOR_zpg_X:
                case OpCode.INC_zpg:
                case OpCode.INC_zpg_X:
                case OpCode.LDA_zpg:
                case OpCode.LDA_zpg_X:
                case OpCode.LDX_zpg:
                case OpCode.LDX_zpg_Y:
                case OpCode.LDY_zpg:
                case OpCode.LDY_zpg_X:
                case OpCode.LSR_zpg:
                case OpCode.LSR_zpg_X:
                case OpCode.ORA_zpg:
                case OpCode.ORA_zpg_X:
                case OpCode.ROL_zpg:
                case OpCode.ROL_zpg_X:
                case OpCode.ROR_zpg:
                case OpCode.ROR_zpg_X:
                case OpCode.SBC_zpg:
                case OpCode.SBC_zpg_X:
                case OpCode.STA_zpg:
                case OpCode.STA_zpg_X:
                case OpCode.STX_zpg:
                case OpCode.STX_zpg_Y:
                case OpCode.STY_zpg:
                case OpCode.STY_zpg_X:
                    return 1;

                default: return 0;
            }
        }

        internal static uint? GetJumpDestination(Instruction instruction)
        {
            switch (instruction.Op)
            {
                case OpCode.BCC_rel:
                case OpCode.BCS_rel:
                case OpCode.BEQ_rel:
                case OpCode.BMI_rel:
                case OpCode.BNE_rel:
                case OpCode.BPL_rel:
                case OpCode.BVC_rel:
                case OpCode.BVS_rel:
                case OpCode.JMP_abs:
                case OpCode.JSR_abs:
                    Word w = instruction.Operands[0] as Word;
                    return w.Absolute;

                default: return null;
            }
        }

        internal static bool IsOpEnd(OpCode op)
        {
            switch (op)
            {
                case OpCode.JMP_abs:
                case OpCode.JMP_ind:
                case OpCode.RTI:
                case OpCode.RTS:
                    return true;

                default: return false;
            }
        }

        public static OpCode? GetOpCode(byte b)
        {
            if (Enum.IsDefined(typeof(OpCode), (int)b)) return (OpCode)b;

            return null;
        }
    }
}
