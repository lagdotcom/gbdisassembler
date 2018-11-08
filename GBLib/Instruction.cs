using System;

namespace GBLib
{
    public class Instruction
    {
        public Instruction(LR35902 cpu, OpCode op, int loc)
        {
            CPU = cpu;
            Location = loc;
            Op = op;
            OperandSize = LR35902.GetOperandSize(op);

            OperandBytes = Tool.Slice(cpu.Memory, loc + 1, loc + 1 + OperandSize);
            IsEnd = LR35902.IsOpEnd(op);
            JumpLocation = LR35902.GetJumpDestination(op, loc + 1, OperandBytes);

            OpType = DetermineOpType(op);
            Operands = DetermineOperands();
        }

        public LR35902 CPU;
        public int Location;
        public OpCode Op;
        public byte[] OperandBytes;

        public string OpType;
        public string[] Operands;

        public int OperandSize;
        public bool IsEnd;
        public int? JumpLocation;
        public int TotalSize => 1 + OperandSize;

        public override string ToString()
        {
            return $"{Location:X6}: {OpType} {string.Join(",", Operands)}";
        }

        private static string DetermineOpType(OpCode op)
        {
            switch (op)
            {
                case OpCode.ADC_A_A:
                case OpCode.ADC_A_B:
                case OpCode.ADC_A_C:
                case OpCode.ADC_A_D:
                case OpCode.ADC_A_d8:
                case OpCode.ADC_A_E:
                case OpCode.ADC_A_H:
                case OpCode.ADC_A_iHL:
                case OpCode.ADC_A_L:
                    return "ADC";

                case OpCode.ADD_A_A:
                case OpCode.ADD_A_B:
                case OpCode.ADD_A_C:
                case OpCode.ADD_A_D:
                case OpCode.ADD_A_d8:
                case OpCode.ADD_A_E:
                case OpCode.ADD_A_H:
                case OpCode.ADD_A_iHL:
                case OpCode.ADD_A_L:
                case OpCode.ADD_HL_BC:
                case OpCode.ADD_HL_DE:
                case OpCode.ADD_HL_HL:
                case OpCode.ADD_HL_SP:
                case OpCode.ADD_SP_r8:
                    return "ADD";

                case OpCode.AND_A:
                case OpCode.AND_B:
                case OpCode.AND_C:
                case OpCode.AND_D:
                case OpCode.AND_d8:
                case OpCode.AND_E:
                case OpCode.AND_H:
                case OpCode.AND_iHL:
                case OpCode.AND_L:
                    return "AND";

                case OpCode.CALL_a16:
                case OpCode.CALL_C_a16:
                case OpCode.CALL_NC_a16:
                case OpCode.CALL_NZ_a16:
                case OpCode.CALL_Z_a16:
                    return "CALL";

                case OpCode.CCF: return "CCF";

                case OpCode.CP_A:
                case OpCode.CP_B:
                case OpCode.CP_C:
                case OpCode.CP_D:
                case OpCode.CP_d8:
                case OpCode.CP_E:
                case OpCode.CP_H:
                case OpCode.CP_iHL:
                case OpCode.CP_L:
                    return "CP";

                case OpCode.CPL: return "CPL";
                case OpCode.DAA: return "DAA";

                case OpCode.DEC_A:
                case OpCode.DEC_B:
                case OpCode.DEC_BC:
                case OpCode.DEC_C:
                case OpCode.DEC_D:
                case OpCode.DEC_DE:
                case OpCode.DEC_E:
                case OpCode.DEC_H:
                case OpCode.DEC_HL:
                case OpCode.DEC_iHL:
                case OpCode.DEC_L:
                case OpCode.DEC_SP:
                    return "DEC";

                case OpCode.DI: return "DI";
                case OpCode.EI: return "EI";
                case OpCode.HALT: return "HALT";

                case OpCode.INC_A:
                case OpCode.INC_B:
                case OpCode.INC_BC:
                case OpCode.INC_C:
                case OpCode.INC_D:
                case OpCode.INC_DE:
                case OpCode.INC_E:
                case OpCode.INC_H:
                case OpCode.INC_HL:
                case OpCode.INC_iHL:
                case OpCode.INC_L:
                case OpCode.INC_SP:
                    return "INC";

                case OpCode.JP_a16:
                case OpCode.JP_C_a16:
                case OpCode.JP_iHL:
                case OpCode.JP_NC_a16:
                case OpCode.JP_NZ_a16:
                case OpCode.JP_Z_a16:
                    return "JP";

                case OpCode.JR_C_r8:
                case OpCode.JR_NC_r8:
                case OpCode.JR_NZ_r8:
                case OpCode.JR_r8:
                case OpCode.JR_Z_r8:
                    return "JR";

                case OpCode.LD_a16_A:
                case OpCode.LD_A_A:
                case OpCode.LD_A_a16:
                case OpCode.LD_A_B:
                case OpCode.LD_A_C:
                case OpCode.LD_A_D:
                case OpCode.LD_A_d8:
                case OpCode.LD_A_E:
                case OpCode.LD_A_H:
                case OpCode.LD_A_iBC:
                case OpCode.LD_A_iC:
                case OpCode.LD_A_iDE:
                case OpCode.LD_A_iHL:
                case OpCode.LD_A_iHLm:
                case OpCode.LD_A_iHLp:
                case OpCode.LD_A_L:
                case OpCode.LD_B_A:
                case OpCode.LD_B_B:
                case OpCode.LD_B_C:
                case OpCode.LD_B_D:
                case OpCode.LD_B_d8:
                case OpCode.LD_B_E:
                case OpCode.LD_B_H:
                case OpCode.LD_B_iHL:
                case OpCode.LD_B_L:
                case OpCode.LD_BC_d16:
                case OpCode.LD_C_A:
                case OpCode.LD_C_B:
                case OpCode.LD_C_C:
                case OpCode.LD_C_D:
                case OpCode.LD_C_d8:
                case OpCode.LD_C_E:
                case OpCode.LD_C_H:
                case OpCode.LD_C_iHL:
                case OpCode.LD_C_L:
                case OpCode.LD_D_A:
                case OpCode.LD_D_B:
                case OpCode.LD_D_C:
                case OpCode.LD_D_D:
                case OpCode.LD_D_d8:
                case OpCode.LD_D_E:
                case OpCode.LD_D_H:
                case OpCode.LD_D_iHL:
                case OpCode.LD_D_L:
                case OpCode.LD_DE_d16:
                case OpCode.LD_E_A:
                case OpCode.LD_E_B:
                case OpCode.LD_E_C:
                case OpCode.LD_E_D:
                case OpCode.LD_E_d8:
                case OpCode.LD_E_E:
                case OpCode.LD_E_H:
                case OpCode.LD_E_iHL:
                case OpCode.LD_E_L:
                case OpCode.LD_H_A:
                case OpCode.LD_H_B:
                case OpCode.LD_H_C:
                case OpCode.LD_H_D:
                case OpCode.LD_H_d8:
                case OpCode.LD_H_E:
                case OpCode.LD_H_H:
                case OpCode.LD_H_iHL:
                case OpCode.LD_H_L:
                case OpCode.LD_HL_d16:
                case OpCode.LD_HL_SPpr8:
                case OpCode.LD_i16_SP:
                case OpCode.LD_iBC_A:
                case OpCode.LD_iC_A:
                case OpCode.LD_iDE_A:
                case OpCode.LD_iHL_A:
                case OpCode.LD_iHL_B:
                case OpCode.LD_iHL_C:
                case OpCode.LD_iHL_D:
                case OpCode.LD_iHL_d8:
                case OpCode.LD_iHL_E:
                case OpCode.LD_iHL_H:
                case OpCode.LD_iHL_L:
                case OpCode.LD_iHLm_A:
                case OpCode.LD_iHLp_A:
                case OpCode.LD_L_A:
                case OpCode.LD_L_B:
                case OpCode.LD_L_C:
                case OpCode.LD_L_D:
                case OpCode.LD_L_d8:
                case OpCode.LD_L_E:
                case OpCode.LD_L_H:
                case OpCode.LD_L_iHL:
                case OpCode.LD_L_L:
                case OpCode.LD_SP_d16:
                case OpCode.LD_SP_HL:
                case OpCode.LD_h8_A:
                case OpCode.LD_A_h8:
                    return "LD";

                case OpCode.NOP: return "NOP";

                case OpCode.OR_A:
                case OpCode.OR_B:
                case OpCode.OR_C:
                case OpCode.OR_D:
                case OpCode.OR_d8:
                case OpCode.OR_E:
                case OpCode.OR_H:
                case OpCode.OR_iHL:
                case OpCode.OR_L:
                    return "OR";

                case OpCode.POP_AF:
                case OpCode.POP_BC:
                case OpCode.POP_DE:
                case OpCode.POP_HL:
                    return "POP";

                // TODO
                case OpCode.PREFIX_CB: return "PREFIX CB";

                case OpCode.PUSH_AF:
                case OpCode.PUSH_BC:
                case OpCode.PUSH_DE:
                case OpCode.PUSH_HL:
                    return "PUSH";

                case OpCode.RET:
                case OpCode.RET_C:
                case OpCode.RET_NC:
                case OpCode.RET_NZ:
                case OpCode.RET_Z:
                    return "RET";

                case OpCode.RETI: return "RETI";
                case OpCode.RLA: return "RLA";
                case OpCode.RLCA: return "RLCA";
                case OpCode.RRA: return "RRA";
                case OpCode.RRCA: return "RRCA";

                case OpCode.RST_00:
                case OpCode.RST_08:
                case OpCode.RST_10:
                case OpCode.RST_18:
                case OpCode.RST_20:
                case OpCode.RST_28:
                case OpCode.RST_30:
                case OpCode.RST_38:
                    return "RST";

                case OpCode.SBC_A_A:
                case OpCode.SBC_A_B:
                case OpCode.SBC_A_C:
                case OpCode.SBC_A_D:
                case OpCode.SBC_A_d8:
                case OpCode.SBC_A_E:
                case OpCode.SBC_A_H:
                case OpCode.SBC_A_iHL:
                case OpCode.SBC_A_L:
                    return "SBC";

                case OpCode.SCF: return "SCF";
                case OpCode.STOP: return "STOP";

                case OpCode.SUB_A:
                case OpCode.SUB_B:
                case OpCode.SUB_C:
                case OpCode.SUB_D:
                case OpCode.SUB_d8:
                case OpCode.SUB_E:
                case OpCode.SUB_H:
                case OpCode.SUB_iHL:
                case OpCode.SUB_L:
                    return "SUB";

                case OpCode.XOR_A:
                case OpCode.XOR_B:
                case OpCode.XOR_C:
                case OpCode.XOR_D:
                case OpCode.XOR_d8:
                case OpCode.XOR_E:
                case OpCode.XOR_H:
                case OpCode.XOR_iHL:
                case OpCode.XOR_L:
                    return "XOR";

                default: return "?";
            }
        }

        private string[] DetermineOperands()
        {
            switch (Op)
            {
                case OpCode.ADC_A_A:
                case OpCode.ADD_A_A:
                case OpCode.LD_A_A:
                case OpCode.SBC_A_A:
                    return S("A", "A");

                case OpCode.ADC_A_B:
                case OpCode.ADD_A_B:
                case OpCode.LD_A_B:
                case OpCode.SBC_A_B:
                    return S("A", "B");

                case OpCode.ADC_A_C:
                case OpCode.ADD_A_C:
                case OpCode.LD_A_C:
                case OpCode.SBC_A_C:
                    return S("A", "C");

                case OpCode.ADC_A_D:
                case OpCode.ADD_A_D:
                case OpCode.LD_A_D:
                case OpCode.SBC_A_D:
                    return S("A", "D");

                case OpCode.ADC_A_d8:
                case OpCode.ADD_A_d8:
                case OpCode.LD_A_d8:
                case OpCode.SBC_A_d8:
                    return S("A", d8);

                case OpCode.ADC_A_E:
                case OpCode.ADD_A_E:
                case OpCode.LD_A_E:
                case OpCode.SBC_A_E:
                    return S("A", "E");

                case OpCode.ADC_A_H:
                case OpCode.ADD_A_H:
                case OpCode.LD_A_H:
                case OpCode.SBC_A_H:
                    return S("A", "H");

                case OpCode.LD_A_iBC: return S("A", "(BC)");
                case OpCode.LD_A_iDE: return S("A", "(DE)");
                case OpCode.LD_A_iHLm: return S("A", "(HL-)");
                case OpCode.LD_A_iHLp: return S("A", "(HL+)");
                case OpCode.LD_A_a16: return S("A", a16);
                case OpCode.LD_a16_A: return S(a16, "A");

                case OpCode.ADC_A_iHL:
                case OpCode.ADD_A_iHL:
                case OpCode.LD_A_iHL:
                case OpCode.SBC_A_iHL:
                    return S("A", "(HL)");

                case OpCode.ADC_A_L:
                case OpCode.ADD_A_L:
                case OpCode.LD_A_L:
                case OpCode.SBC_A_L:
                    return S("A", "L");

                case OpCode.ADD_HL_BC: return S("HL", "BC");
                case OpCode.ADD_HL_DE: return S("HL", "DE");
                case OpCode.ADD_HL_HL: return S("HL", "HL");
                case OpCode.ADD_HL_SP: return S("HL", "SP");
                case OpCode.ADD_SP_r8: return S("SP", r8);

                case OpCode.AND_A:
                case OpCode.CP_A:
                case OpCode.DEC_A:
                case OpCode.INC_A:
                case OpCode.OR_A:
                case OpCode.SUB_A:
                case OpCode.XOR_A:
                    return S("A");

                case OpCode.AND_B:
                case OpCode.CP_B:
                case OpCode.DEC_B:
                case OpCode.INC_B:
                case OpCode.OR_B:
                case OpCode.SUB_B:
                case OpCode.XOR_B:
                    return S("B");

                case OpCode.AND_C:
                case OpCode.CP_C:
                case OpCode.DEC_C:
                case OpCode.INC_C:
                case OpCode.OR_C:
                case OpCode.SUB_C:
                case OpCode.XOR_C:
                    return S("C");

                case OpCode.AND_D:
                case OpCode.CP_D:
                case OpCode.DEC_D:
                case OpCode.INC_D:
                case OpCode.OR_D:
                case OpCode.SUB_D:
                case OpCode.XOR_D:
                    return S("D");

                case OpCode.AND_E:
                case OpCode.CP_E:
                case OpCode.DEC_E:
                case OpCode.INC_E:
                case OpCode.OR_E:
                case OpCode.SUB_E:
                case OpCode.XOR_E:
                    return S("E");

                case OpCode.AND_H:
                case OpCode.CP_H:
                case OpCode.DEC_H:
                case OpCode.INC_H:
                case OpCode.OR_H:
                case OpCode.SUB_H:
                case OpCode.XOR_H:
                    return S("H");

                case OpCode.AND_L:
                case OpCode.CP_L:
                case OpCode.DEC_L:
                case OpCode.INC_L:
                case OpCode.OR_L:
                case OpCode.SUB_L:
                case OpCode.XOR_L:
                    return S("L");

                case OpCode.AND_d8:
                case OpCode.CP_d8:
                case OpCode.OR_d8:
                case OpCode.SUB_d8:
                case OpCode.XOR_d8:
                    return S(d8);

                case OpCode.PUSH_AF:
                case OpCode.POP_AF:
                    return S("AF");

                case OpCode.DEC_BC:
                case OpCode.INC_BC:
                case OpCode.POP_BC:
                case OpCode.PUSH_BC:
                    return S("BC");

                case OpCode.DEC_DE:
                case OpCode.INC_DE:
                case OpCode.POP_DE:
                case OpCode.PUSH_DE:
                    return S("DE");

                case OpCode.DEC_HL:
                case OpCode.INC_HL:
                case OpCode.POP_HL:
                case OpCode.PUSH_HL:
                    return S("HL");

                case OpCode.AND_iHL:
                case OpCode.CP_iHL:
                case OpCode.DEC_iHL:
                case OpCode.INC_iHL:
                case OpCode.JP_iHL:
                case OpCode.OR_iHL:
                case OpCode.SUB_iHL:
                case OpCode.XOR_iHL:
                    return S("(HL)");

                case OpCode.DEC_SP:
                case OpCode.INC_SP:
                    return S("SP");

                case OpCode.RST_00: return S("00");
                case OpCode.RST_08: return S("08");
                case OpCode.RST_10: return S("10");
                case OpCode.RST_18: return S("18");
                case OpCode.RST_20: return S("20");
                case OpCode.RST_28: return S("28");
                case OpCode.RST_30: return S("30");
                case OpCode.RST_38: return S("38");

                case OpCode.CALL_a16:
                case OpCode.JP_a16:
                    return S(a16);

                case OpCode.CALL_C_a16:
                case OpCode.JP_C_a16:
                    return S("c", a16);

                case OpCode.CALL_NC_a16:
                case OpCode.JP_NC_a16:
                    return S("nc", a16);

                case OpCode.CALL_NZ_a16:
                case OpCode.JP_NZ_a16:
                    return S("nz", a16);

                case OpCode.CALL_Z_a16:
                case OpCode.JP_Z_a16:
                    return S("z", a16);

                case OpCode.RET_C: return S("c");
                case OpCode.RET_NC: return S("nc");
                case OpCode.RET_NZ: return S("nz");
                case OpCode.RET_Z: return S("z");

                case OpCode.JR_r8: return S(r8);
                case OpCode.JR_C_r8: return S("c", r8);
                case OpCode.JR_NC_r8: return S("nc", r8);
                case OpCode.JR_NZ_r8: return S("nz", r8);
                case OpCode.JR_Z_r8: return S("z", r8);

                case OpCode.LD_A_h8: return S("A", h8);
                case OpCode.LD_h8_A: return S(h8, "A");

                case OpCode.LD_A_iC: return S("A", "($FF00+C)");
                case OpCode.LD_iC_A: return S("($FF00+C)", "A");

                case OpCode.LD_B_A: return S("B", "A");
                case OpCode.LD_B_B: return S("B", "B");
                case OpCode.LD_B_C: return S("B", "C");
                case OpCode.LD_B_D: return S("B", "D");
                case OpCode.LD_B_d8: return S("B", d8);
                case OpCode.LD_B_E: return S("B", "E");
                case OpCode.LD_B_H: return S("B", "H");
                case OpCode.LD_B_iHL: return S("B", "(HL)");
                case OpCode.LD_B_L: return S("B", "L");
                case OpCode.LD_C_A: return S("C", "A");
                case OpCode.LD_C_B: return S("C", "B");
                case OpCode.LD_C_C: return S("C", "C");
                case OpCode.LD_C_D: return S("C", "D");
                case OpCode.LD_C_d8: return S("C", d8);
                case OpCode.LD_C_E: return S("C", "E");
                case OpCode.LD_C_H: return S("C", "H");
                case OpCode.LD_C_iHL: return S("C", "(HL)");
                case OpCode.LD_C_L: return S("C", "L");
                case OpCode.LD_D_A: return S("D", "A");
                case OpCode.LD_D_B: return S("D", "B");
                case OpCode.LD_D_C: return S("D", "C");
                case OpCode.LD_D_D: return S("D", "D");
                case OpCode.LD_D_d8: return S("D", d8);
                case OpCode.LD_D_E: return S("D", "E");
                case OpCode.LD_D_H: return S("D", "H");
                case OpCode.LD_D_iHL: return S("D", "(HL)");
                case OpCode.LD_D_L: return S("D", "L");
                case OpCode.LD_E_A: return S("E", "A");
                case OpCode.LD_E_B: return S("E", "B");
                case OpCode.LD_E_C: return S("E", "C");
                case OpCode.LD_E_D: return S("E", "D");
                case OpCode.LD_E_d8: return S("E", d8);
                case OpCode.LD_E_E: return S("E", "E");
                case OpCode.LD_E_H: return S("E", "H");
                case OpCode.LD_E_iHL: return S("E", "(HL)");
                case OpCode.LD_E_L: return S("E", "L");
                case OpCode.LD_H_A: return S("H", "A");
                case OpCode.LD_H_B: return S("H", "B");
                case OpCode.LD_H_C: return S("H", "C");
                case OpCode.LD_H_D: return S("H", "D");
                case OpCode.LD_H_d8: return S("H", d8);
                case OpCode.LD_H_E: return S("H", "E");
                case OpCode.LD_H_H: return S("H", "H");
                case OpCode.LD_H_iHL: return S("H", "(HL)");
                case OpCode.LD_H_L: return S("H", "L");
                case OpCode.LD_HL_SPpr8: return S("HL", $"SP+{d8}");
                case OpCode.LD_i16_SP: return S($"({a16})", "SP");
                case OpCode.LD_iBC_A: return S("(BC)", "A");
                case OpCode.LD_iDE_A: return S("(DE)", "A");
                case OpCode.LD_iHL_A: return S("(HL)", "A");
                case OpCode.LD_iHL_B: return S("(HL)", "B");
                case OpCode.LD_iHL_C: return S("(HL)", "C");
                case OpCode.LD_iHL_D: return S("(HL)", "D");
                case OpCode.LD_iHL_d8: return S("(HL)", d8);
                case OpCode.LD_iHL_E: return S("(HL)", "E");
                case OpCode.LD_iHL_H: return S("(HL)", "H");
                case OpCode.LD_iHL_L: return S("(HL)", "L");
                case OpCode.LD_iHLm_A: return S("(HL-)", "A");
                case OpCode.LD_iHLp_A: return S("(HL+)", "A");
                case OpCode.LD_L_A: return S("L", "A");
                case OpCode.LD_L_B: return S("L", "B");
                case OpCode.LD_L_C: return S("L", "C");
                case OpCode.LD_L_D: return S("L", "D");
                case OpCode.LD_L_d8: return S("L", d8);
                case OpCode.LD_L_E: return S("L", "E");
                case OpCode.LD_L_H: return S("L", "H");
                case OpCode.LD_L_iHL: return S("L", "(HL)");
                case OpCode.LD_L_L: return S("L", "L");
                case OpCode.LD_SP_HL: return S("SP", "HL");

                case OpCode.LD_BC_d16: return S("BC", d16);
                case OpCode.LD_DE_d16: return S("DE", d16);
                case OpCode.LD_HL_d16: return S("HL", d16);
                case OpCode.LD_SP_d16: return S("SP", d16);

                default: return S();
            }
        }

        private static string[] S(params string[] parts) => parts;
        private static string SA(int i) => $"${i:X4}";
        private static string SB(byte b) => $"${b:X2}";
        private string a16 => SA(BitConverter.ToUInt16(OperandBytes, 0));
        private string d8 => SB(OperandBytes[0]);
        private string d16 => SA(BitConverter.ToUInt16(OperandBytes, 0));
        private string h8 => SA(0xFF00 + OperandBytes[0]);
        private string r8 => SA(Location + 2 + (sbyte)OperandBytes[0]);
    }
}
