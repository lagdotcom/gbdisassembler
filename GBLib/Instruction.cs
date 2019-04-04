using System;
using System.Collections.Generic;
using System.Linq;
using GBLib.Operand;

namespace GBLib
{
    public class Instruction
    {
        public Instruction() { }
        public Instruction(Disassembler dis, OpCode op, uint loc) : this()
        {
            Parent = dis;
            Location = loc;
            Address = new BankedAddress(loc);
            Op = op;
            OperandSize = LR35902.GetOperandSize(op);

            OperandBytes = Tool.Slice(Parent.ROM, loc + 1, loc + TotalSize);
            IsEnd = LR35902.IsOpEnd(op);
            JumpLocation = LR35902.GetJumpDestination(op, loc + 1, OperandBytes);

            if (Op == OpCode.PREFIX_CB)
                CBOp = (CBOpCode)OperandBytes[0];

            OpType = DetermineOpType(Op, CBOp);
            Operands = DetermineOperands();
        }

        public BankedAddress Address;
        public Disassembler Parent;
        public uint Location;
        public OpCode Op;
        public CBOpCode? CBOp;
        public byte[] OperandBytes;

        public string OpType;
        public IOperand[] Operands;

        public uint OperandSize;
        public bool IsEnd;
        public uint? JumpLocation;
        public uint TotalSize => 1 + OperandSize;

        public override string ToString()
        {
            IEnumerable<string> OperandStrings = Operands.Select(op => ImproveOperand(op));

            return $"{Address} | {OpType} {string.Join(",", OperandStrings)}".Trim();
        }

        private string ImproveOperand(IOperand op)
        {
            IPortHandler handler = Parent.FindHandler(op);
            if (handler != null)
                return handler.Identify(op.AbsoluteAddress.Value);

            if (op is BankedAddress && Address.Bank > 1)
            {
                BankedAddress ba = op as BankedAddress;
                if (ba.Bank == 1) return ba.OverrideBankString(Address.Bank);
            }

            return op.ToString();
        }

        private static string DetermineOpType(OpCode op, CBOpCode? cb)
        {
            if (op == OpCode.PREFIX_CB)
                switch (cb)
                {
                    case CBOpCode.RLC_A: case CBOpCode.RLC_B: case CBOpCode.RLC_C: case CBOpCode.RLC_D:
                    case CBOpCode.RLC_E: case CBOpCode.RLC_H: case CBOpCode.RLC_iHL: case CBOpCode.RLC_L:
                        return "RLC";

                    case CBOpCode.RL_A: case CBOpCode.RL_B: case CBOpCode.RL_C: case CBOpCode.RL_D:
                    case CBOpCode.RL_E: case CBOpCode.RL_H: case CBOpCode.RL_iHL: case CBOpCode.RL_L:
                        return "RL";

                    case CBOpCode.RRC_A: case CBOpCode.RRC_B: case CBOpCode.RRC_C: case CBOpCode.RRC_D:
                    case CBOpCode.RRC_E: case CBOpCode.RRC_H: case CBOpCode.RRC_iHL: case CBOpCode.RRC_L:
                        return "RRC";

                    case CBOpCode.RR_A: case CBOpCode.RR_B: case CBOpCode.RR_C: case CBOpCode.RR_D:
                    case CBOpCode.RR_E: case CBOpCode.RR_H: case CBOpCode.RR_iHL: case CBOpCode.RR_L:
                        return "RR";

                    case CBOpCode.SLA_A: case CBOpCode.SLA_B: case CBOpCode.SLA_C: case CBOpCode.SLA_D:
                    case CBOpCode.SLA_E: case CBOpCode.SLA_H: case CBOpCode.SLA_iHL: case CBOpCode.SLA_L:
                        return "SLA";

                    case CBOpCode.SRA_A: case CBOpCode.SRA_B: case CBOpCode.SRA_C: case CBOpCode.SRA_D:
                    case CBOpCode.SRA_E: case CBOpCode.SRA_H: case CBOpCode.SRA_iHL: case CBOpCode.SRA_L:
                        return "SRA";

                    case CBOpCode.SRL_A: case CBOpCode.SRL_B: case CBOpCode.SRL_C: case CBOpCode.SRL_D:
                    case CBOpCode.SRL_E: case CBOpCode.SRL_H: case CBOpCode.SRL_iHL: case CBOpCode.SRL_L:
                        return "SRL";

                    case CBOpCode.SWAP_A: case CBOpCode.SWAP_B: case CBOpCode.SWAP_C: case CBOpCode.SWAP_D:
                    case CBOpCode.SWAP_E: case CBOpCode.SWAP_H: case CBOpCode.SWAP_iHL: case CBOpCode.SWAP_L:
                        return "SWAP";

                    case CBOpCode.BIT_0_A: case CBOpCode.BIT_1_A: case CBOpCode.BIT_2_A: case CBOpCode.BIT_3_A:
                    case CBOpCode.BIT_4_A: case CBOpCode.BIT_5_A: case CBOpCode.BIT_6_A: case CBOpCode.BIT_7_A:
                    case CBOpCode.BIT_0_B: case CBOpCode.BIT_1_B: case CBOpCode.BIT_2_B: case CBOpCode.BIT_3_B:
                    case CBOpCode.BIT_4_B: case CBOpCode.BIT_5_B: case CBOpCode.BIT_6_B: case CBOpCode.BIT_7_B:
                    case CBOpCode.BIT_0_C: case CBOpCode.BIT_1_C: case CBOpCode.BIT_2_C: case CBOpCode.BIT_3_C:
                    case CBOpCode.BIT_4_C: case CBOpCode.BIT_5_C: case CBOpCode.BIT_6_C: case CBOpCode.BIT_7_C:
                    case CBOpCode.BIT_0_D: case CBOpCode.BIT_1_D: case CBOpCode.BIT_2_D: case CBOpCode.BIT_3_D:
                    case CBOpCode.BIT_4_D: case CBOpCode.BIT_5_D: case CBOpCode.BIT_6_D: case CBOpCode.BIT_7_D:
                    case CBOpCode.BIT_0_E: case CBOpCode.BIT_1_E: case CBOpCode.BIT_2_E: case CBOpCode.BIT_3_E:
                    case CBOpCode.BIT_4_E: case CBOpCode.BIT_5_E: case CBOpCode.BIT_6_E: case CBOpCode.BIT_7_E:
                    case CBOpCode.BIT_0_H: case CBOpCode.BIT_1_H: case CBOpCode.BIT_2_H: case CBOpCode.BIT_3_H:
                    case CBOpCode.BIT_4_H: case CBOpCode.BIT_5_H: case CBOpCode.BIT_6_H: case CBOpCode.BIT_7_H:
                    case CBOpCode.BIT_0_L: case CBOpCode.BIT_1_L: case CBOpCode.BIT_2_L: case CBOpCode.BIT_3_L:
                    case CBOpCode.BIT_4_L: case CBOpCode.BIT_5_L: case CBOpCode.BIT_6_L: case CBOpCode.BIT_7_L:
                    case CBOpCode.BIT_0_iHL: case CBOpCode.BIT_1_iHL: case CBOpCode.BIT_2_iHL: case CBOpCode.BIT_3_iHL:
                    case CBOpCode.BIT_4_iHL: case CBOpCode.BIT_5_iHL: case CBOpCode.BIT_6_iHL: case CBOpCode.BIT_7_iHL:
                        return "BIT";

                    case CBOpCode.RES_0_A: case CBOpCode.RES_1_A: case CBOpCode.RES_2_A: case CBOpCode.RES_3_A:
                    case CBOpCode.RES_4_A: case CBOpCode.RES_5_A: case CBOpCode.RES_6_A: case CBOpCode.RES_7_A:
                    case CBOpCode.RES_0_B: case CBOpCode.RES_1_B: case CBOpCode.RES_2_B: case CBOpCode.RES_3_B:
                    case CBOpCode.RES_4_B: case CBOpCode.RES_5_B: case CBOpCode.RES_6_B: case CBOpCode.RES_7_B:
                    case CBOpCode.RES_0_C: case CBOpCode.RES_1_C: case CBOpCode.RES_2_C: case CBOpCode.RES_3_C:
                    case CBOpCode.RES_4_C: case CBOpCode.RES_5_C: case CBOpCode.RES_6_C: case CBOpCode.RES_7_C:
                    case CBOpCode.RES_0_D: case CBOpCode.RES_1_D: case CBOpCode.RES_2_D: case CBOpCode.RES_3_D:
                    case CBOpCode.RES_4_D: case CBOpCode.RES_5_D: case CBOpCode.RES_6_D: case CBOpCode.RES_7_D:
                    case CBOpCode.RES_0_E: case CBOpCode.RES_1_E: case CBOpCode.RES_2_E: case CBOpCode.RES_3_E:
                    case CBOpCode.RES_4_E: case CBOpCode.RES_5_E: case CBOpCode.RES_6_E: case CBOpCode.RES_7_E:
                    case CBOpCode.RES_0_H: case CBOpCode.RES_1_H: case CBOpCode.RES_2_H: case CBOpCode.RES_3_H:
                    case CBOpCode.RES_4_H: case CBOpCode.RES_5_H: case CBOpCode.RES_6_H: case CBOpCode.RES_7_H:
                    case CBOpCode.RES_0_L: case CBOpCode.RES_1_L: case CBOpCode.RES_2_L: case CBOpCode.RES_3_L:
                    case CBOpCode.RES_4_L: case CBOpCode.RES_5_L: case CBOpCode.RES_6_L: case CBOpCode.RES_7_L:
                    case CBOpCode.RES_0_iHL: case CBOpCode.RES_1_iHL: case CBOpCode.RES_2_iHL: case CBOpCode.RES_3_iHL:
                    case CBOpCode.RES_4_iHL: case CBOpCode.RES_5_iHL: case CBOpCode.RES_6_iHL: case CBOpCode.RES_7_iHL:
                        return "RES";

                    case CBOpCode.SET_0_A: case CBOpCode.SET_1_A: case CBOpCode.SET_2_A: case CBOpCode.SET_3_A:
                    case CBOpCode.SET_4_A: case CBOpCode.SET_5_A: case CBOpCode.SET_6_A: case CBOpCode.SET_7_A:
                    case CBOpCode.SET_0_B: case CBOpCode.SET_1_B: case CBOpCode.SET_2_B: case CBOpCode.SET_3_B:
                    case CBOpCode.SET_4_B: case CBOpCode.SET_5_B: case CBOpCode.SET_6_B: case CBOpCode.SET_7_B:
                    case CBOpCode.SET_0_C: case CBOpCode.SET_1_C: case CBOpCode.SET_2_C: case CBOpCode.SET_3_C:
                    case CBOpCode.SET_4_C: case CBOpCode.SET_5_C: case CBOpCode.SET_6_C: case CBOpCode.SET_7_C:
                    case CBOpCode.SET_0_D: case CBOpCode.SET_1_D: case CBOpCode.SET_2_D: case CBOpCode.SET_3_D:
                    case CBOpCode.SET_4_D: case CBOpCode.SET_5_D: case CBOpCode.SET_6_D: case CBOpCode.SET_7_D:
                    case CBOpCode.SET_0_E: case CBOpCode.SET_1_E: case CBOpCode.SET_2_E: case CBOpCode.SET_3_E:
                    case CBOpCode.SET_4_E: case CBOpCode.SET_5_E: case CBOpCode.SET_6_E: case CBOpCode.SET_7_E:
                    case CBOpCode.SET_0_H: case CBOpCode.SET_1_H: case CBOpCode.SET_2_H: case CBOpCode.SET_3_H:
                    case CBOpCode.SET_4_H: case CBOpCode.SET_5_H: case CBOpCode.SET_6_H: case CBOpCode.SET_7_H:
                    case CBOpCode.SET_0_L: case CBOpCode.SET_1_L: case CBOpCode.SET_2_L: case CBOpCode.SET_3_L:
                    case CBOpCode.SET_4_L: case CBOpCode.SET_5_L: case CBOpCode.SET_6_L: case CBOpCode.SET_7_L:
                    case CBOpCode.SET_0_iHL: case CBOpCode.SET_1_iHL: case CBOpCode.SET_2_iHL: case CBOpCode.SET_3_iHL:
                    case CBOpCode.SET_4_iHL: case CBOpCode.SET_5_iHL: case CBOpCode.SET_6_iHL: case CBOpCode.SET_7_iHL:
                        return "SET";
                }

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

        private IOperand[] DetermineOperands()
        {
            if (Op == OpCode.PREFIX_CB)
                switch (CBOp)
                {
                    case CBOpCode.BIT_0_A:
                    case CBOpCode.RES_0_A:
                    case CBOpCode.SET_0_A:
                        return Ops(OpP(0), CPUReg.A);

                    case CBOpCode.BIT_1_A:
                    case CBOpCode.RES_1_A:
                    case CBOpCode.SET_1_A:
                        return Ops(OpP(1), CPUReg.A);

                    case CBOpCode.BIT_2_A:
                    case CBOpCode.RES_2_A:
                    case CBOpCode.SET_2_A:
                        return Ops(OpP(2), CPUReg.A);

                    case CBOpCode.BIT_3_A:
                    case CBOpCode.RES_3_A:
                    case CBOpCode.SET_3_A:
                        return Ops(OpP(3), CPUReg.A);

                    case CBOpCode.BIT_4_A:
                    case CBOpCode.RES_4_A:
                    case CBOpCode.SET_4_A:
                        return Ops(OpP(4), CPUReg.A);

                    case CBOpCode.BIT_5_A:
                    case CBOpCode.RES_5_A:
                    case CBOpCode.SET_5_A:
                        return Ops(OpP(5), CPUReg.A);

                    case CBOpCode.BIT_6_A:
                    case CBOpCode.RES_6_A:
                    case CBOpCode.SET_6_A:
                        return Ops(OpP(6), CPUReg.A);

                    case CBOpCode.BIT_7_A:
                    case CBOpCode.RES_7_A:
                    case CBOpCode.SET_7_A:
                        return Ops(OpP(7), CPUReg.A);

                    case CBOpCode.BIT_0_B:
                    case CBOpCode.RES_0_B:
                    case CBOpCode.SET_0_B:
                        return Ops(OpP(0), CPUReg.B);

                    case CBOpCode.BIT_1_B:
                    case CBOpCode.RES_1_B:
                    case CBOpCode.SET_1_B:
                        return Ops(OpP(1), CPUReg.B);

                    case CBOpCode.BIT_2_B:
                    case CBOpCode.RES_2_B:
                    case CBOpCode.SET_2_B:
                        return Ops(OpP(2), CPUReg.B);

                    case CBOpCode.BIT_3_B:
                    case CBOpCode.RES_3_B:
                    case CBOpCode.SET_3_B:
                        return Ops(OpP(3), CPUReg.B);

                    case CBOpCode.BIT_4_B:
                    case CBOpCode.RES_4_B:
                    case CBOpCode.SET_4_B:
                        return Ops(OpP(4), CPUReg.B);

                    case CBOpCode.BIT_5_B:
                    case CBOpCode.RES_5_B:
                    case CBOpCode.SET_5_B:
                        return Ops(OpP(5), CPUReg.B);

                    case CBOpCode.BIT_6_B:
                    case CBOpCode.RES_6_B:
                    case CBOpCode.SET_6_B:
                        return Ops(OpP(6), CPUReg.B);

                    case CBOpCode.BIT_7_B:
                    case CBOpCode.RES_7_B:
                    case CBOpCode.SET_7_B:
                        return Ops(OpP(7), CPUReg.B);

                    case CBOpCode.BIT_0_C:
                    case CBOpCode.RES_0_C:
                    case CBOpCode.SET_0_C:
                        return Ops(OpP(0), CPUReg.C);

                    case CBOpCode.BIT_1_C:
                    case CBOpCode.RES_1_C:
                    case CBOpCode.SET_1_C:
                        return Ops(OpP(1), CPUReg.C);

                    case CBOpCode.BIT_2_C:
                    case CBOpCode.RES_2_C:
                    case CBOpCode.SET_2_C:
                        return Ops(OpP(2), CPUReg.C);

                    case CBOpCode.BIT_3_C:
                    case CBOpCode.RES_3_C:
                    case CBOpCode.SET_3_C:
                        return Ops(OpP(3), CPUReg.C);

                    case CBOpCode.BIT_4_C:
                    case CBOpCode.RES_4_C:
                    case CBOpCode.SET_4_C:
                        return Ops(OpP(4), CPUReg.C);

                    case CBOpCode.BIT_5_C:
                    case CBOpCode.RES_5_C:
                    case CBOpCode.SET_5_C:
                        return Ops(OpP(5), CPUReg.C);

                    case CBOpCode.BIT_6_C:
                    case CBOpCode.RES_6_C:
                    case CBOpCode.SET_6_C:
                        return Ops(OpP(6), CPUReg.C);

                    case CBOpCode.BIT_7_C:
                    case CBOpCode.RES_7_C:
                    case CBOpCode.SET_7_C:
                        return Ops(OpP(7), CPUReg.C);

                    case CBOpCode.BIT_0_D:
                    case CBOpCode.RES_0_D:
                    case CBOpCode.SET_0_D:
                        return Ops(OpP(0), CPUReg.D);

                    case CBOpCode.BIT_1_D:
                    case CBOpCode.RES_1_D:
                    case CBOpCode.SET_1_D:
                        return Ops(OpP(1), CPUReg.D);

                    case CBOpCode.BIT_2_D:
                    case CBOpCode.RES_2_D:
                    case CBOpCode.SET_2_D:
                        return Ops(OpP(2), CPUReg.D);

                    case CBOpCode.BIT_3_D:
                    case CBOpCode.RES_3_D:
                    case CBOpCode.SET_3_D:
                        return Ops(OpP(3), CPUReg.D);

                    case CBOpCode.BIT_4_D:
                    case CBOpCode.RES_4_D:
                    case CBOpCode.SET_4_D:
                        return Ops(OpP(4), CPUReg.D);

                    case CBOpCode.BIT_5_D:
                    case CBOpCode.RES_5_D:
                    case CBOpCode.SET_5_D:
                        return Ops(OpP(5), CPUReg.D);

                    case CBOpCode.BIT_6_D:
                    case CBOpCode.RES_6_D:
                    case CBOpCode.SET_6_D:
                        return Ops(OpP(6), CPUReg.D);

                    case CBOpCode.BIT_7_D:
                    case CBOpCode.RES_7_D:
                    case CBOpCode.SET_7_D:
                        return Ops(OpP(7), CPUReg.D);

                    case CBOpCode.BIT_0_E:
                    case CBOpCode.RES_0_E:
                    case CBOpCode.SET_0_E:
                        return Ops(OpP(0), CPUReg.E);

                    case CBOpCode.BIT_1_E:
                    case CBOpCode.RES_1_E:
                    case CBOpCode.SET_1_E:
                        return Ops(OpP(1), CPUReg.E);

                    case CBOpCode.BIT_2_E:
                    case CBOpCode.RES_2_E:
                    case CBOpCode.SET_2_E:
                        return Ops(OpP(2), CPUReg.E);

                    case CBOpCode.BIT_3_E:
                    case CBOpCode.RES_3_E:
                    case CBOpCode.SET_3_E:
                        return Ops(OpP(3), CPUReg.E);

                    case CBOpCode.BIT_4_E:
                    case CBOpCode.RES_4_E:
                    case CBOpCode.SET_4_E:
                        return Ops(OpP(4), CPUReg.E);

                    case CBOpCode.BIT_5_E:
                    case CBOpCode.RES_5_E:
                    case CBOpCode.SET_5_E:
                        return Ops(OpP(5), CPUReg.E);

                    case CBOpCode.BIT_6_E:
                    case CBOpCode.RES_6_E:
                    case CBOpCode.SET_6_E:
                        return Ops(OpP(6), CPUReg.E);

                    case CBOpCode.BIT_7_E:
                    case CBOpCode.RES_7_E:
                    case CBOpCode.SET_7_E:
                        return Ops(OpP(7), CPUReg.E);

                    case CBOpCode.BIT_0_H:
                    case CBOpCode.RES_0_H:
                    case CBOpCode.SET_0_H:
                        return Ops(OpP(0), CPUReg.H);

                    case CBOpCode.BIT_1_H:
                    case CBOpCode.RES_1_H:
                    case CBOpCode.SET_1_H:
                        return Ops(OpP(1), CPUReg.H);

                    case CBOpCode.BIT_2_H:
                    case CBOpCode.RES_2_H:
                    case CBOpCode.SET_2_H:
                        return Ops(OpP(2), CPUReg.H);

                    case CBOpCode.BIT_3_H:
                    case CBOpCode.RES_3_H:
                    case CBOpCode.SET_3_H:
                        return Ops(OpP(3), CPUReg.H);

                    case CBOpCode.BIT_4_H:
                    case CBOpCode.RES_4_H:
                    case CBOpCode.SET_4_H:
                        return Ops(OpP(4), CPUReg.H);

                    case CBOpCode.BIT_5_H:
                    case CBOpCode.RES_5_H:
                    case CBOpCode.SET_5_H:
                        return Ops(OpP(5), CPUReg.H);

                    case CBOpCode.BIT_6_H:
                    case CBOpCode.RES_6_H:
                    case CBOpCode.SET_6_H:
                        return Ops(OpP(6), CPUReg.H);

                    case CBOpCode.BIT_7_H:
                    case CBOpCode.RES_7_H:
                    case CBOpCode.SET_7_H:
                        return Ops(OpP(7), CPUReg.H);


                    case CBOpCode.BIT_0_L:
                    case CBOpCode.RES_0_L:
                    case CBOpCode.SET_0_L:
                        return Ops(OpP(0), CPUReg.L);

                    case CBOpCode.BIT_1_L:
                    case CBOpCode.RES_1_L:
                    case CBOpCode.SET_1_L:
                        return Ops(OpP(1), CPUReg.L);

                    case CBOpCode.BIT_2_L:
                    case CBOpCode.RES_2_L:
                    case CBOpCode.SET_2_L:
                        return Ops(OpP(2), CPUReg.L);

                    case CBOpCode.BIT_3_L:
                    case CBOpCode.RES_3_L:
                    case CBOpCode.SET_3_L:
                        return Ops(OpP(3), CPUReg.L);

                    case CBOpCode.BIT_4_L:
                    case CBOpCode.RES_4_L:
                    case CBOpCode.SET_4_L:
                        return Ops(OpP(4), CPUReg.L);

                    case CBOpCode.BIT_5_L:
                    case CBOpCode.RES_5_L:
                    case CBOpCode.SET_5_L:
                        return Ops(OpP(5), CPUReg.L);

                    case CBOpCode.BIT_6_L:
                    case CBOpCode.RES_6_L:
                    case CBOpCode.SET_6_L:
                        return Ops(OpP(6), CPUReg.L);

                    case CBOpCode.BIT_7_L:
                    case CBOpCode.RES_7_L:
                    case CBOpCode.SET_7_L:
                        return Ops(OpP(7), CPUReg.L);

                    case CBOpCode.BIT_0_iHL:
                    case CBOpCode.RES_0_iHL:
                    case CBOpCode.SET_0_iHL:
                        return Ops(OpP(0), CPUReg.HLI);

                    case CBOpCode.BIT_1_iHL:
                    case CBOpCode.RES_1_iHL:
                    case CBOpCode.SET_1_iHL:
                        return Ops(OpP(1), CPUReg.HLI);

                    case CBOpCode.BIT_2_iHL:
                    case CBOpCode.RES_2_iHL:
                    case CBOpCode.SET_2_iHL:
                        return Ops(OpP(2), CPUReg.HLI);

                    case CBOpCode.BIT_3_iHL:
                    case CBOpCode.RES_3_iHL:
                    case CBOpCode.SET_3_iHL:
                        return Ops(OpP(3), CPUReg.HLI);

                    case CBOpCode.BIT_4_iHL:
                    case CBOpCode.RES_4_iHL:
                    case CBOpCode.SET_4_iHL:
                        return Ops(OpP(4), CPUReg.HLI);

                    case CBOpCode.BIT_5_iHL:
                    case CBOpCode.RES_5_iHL:
                    case CBOpCode.SET_5_iHL:
                        return Ops(OpP(5), CPUReg.HLI);

                    case CBOpCode.BIT_6_iHL:
                    case CBOpCode.RES_6_iHL:
                    case CBOpCode.SET_6_iHL:
                        return Ops(OpP(6), CPUReg.HLI);

                    case CBOpCode.BIT_7_iHL:
                    case CBOpCode.RES_7_iHL:
                    case CBOpCode.SET_7_iHL:
                        return Ops(OpP(7), CPUReg.HLI);

                    case CBOpCode.RLC_A:
                    case CBOpCode.RL_A:
                    case CBOpCode.RRC_A:
                    case CBOpCode.RR_A:
                    case CBOpCode.SLA_A:
                    case CBOpCode.SRA_A:
                    case CBOpCode.SRL_A:
                    case CBOpCode.SWAP_A:
                        return Ops(CPUReg.A);

                    case CBOpCode.RLC_B:
                    case CBOpCode.RL_B:
                    case CBOpCode.RRC_B:
                    case CBOpCode.RR_B:
                    case CBOpCode.SLA_B:
                    case CBOpCode.SRA_B:
                    case CBOpCode.SRL_B:
                    case CBOpCode.SWAP_B:
                        return Ops(CPUReg.B);

                    case CBOpCode.RLC_C:
                    case CBOpCode.RL_C:
                    case CBOpCode.RRC_C:
                    case CBOpCode.RR_C:
                    case CBOpCode.SLA_C:
                    case CBOpCode.SRA_C:
                    case CBOpCode.SRL_C:
                    case CBOpCode.SWAP_C:
                        return Ops(CPUReg.C);

                    case CBOpCode.RLC_D:
                    case CBOpCode.RL_D:
                    case CBOpCode.RRC_D:
                    case CBOpCode.RR_D:
                    case CBOpCode.SLA_D:
                    case CBOpCode.SRA_D:
                    case CBOpCode.SRL_D:
                    case CBOpCode.SWAP_D:
                        return Ops(CPUReg.D);

                    case CBOpCode.RLC_E:
                    case CBOpCode.RL_E:
                    case CBOpCode.RRC_E:
                    case CBOpCode.RR_E:
                    case CBOpCode.SLA_E:
                    case CBOpCode.SRA_E:
                    case CBOpCode.SRL_E:
                    case CBOpCode.SWAP_E:
                        return Ops(CPUReg.E);

                    case CBOpCode.RLC_H:
                    case CBOpCode.RL_H:
                    case CBOpCode.RRC_H:
                    case CBOpCode.RR_H:
                    case CBOpCode.SLA_H:
                    case CBOpCode.SRA_H:
                    case CBOpCode.SRL_H:
                    case CBOpCode.SWAP_H:
                        return Ops(CPUReg.H);

                    case CBOpCode.RLC_L:
                    case CBOpCode.RL_L:
                    case CBOpCode.RRC_L:
                    case CBOpCode.RR_L:
                    case CBOpCode.SLA_L:
                    case CBOpCode.SRA_L:
                    case CBOpCode.SRL_L:
                    case CBOpCode.SWAP_L:
                        return Ops(CPUReg.L);

                    case CBOpCode.RLC_iHL:
                    case CBOpCode.RL_iHL:
                    case CBOpCode.RRC_iHL:
                    case CBOpCode.RR_iHL:
                    case CBOpCode.SLA_iHL:
                    case CBOpCode.SRA_iHL:
                    case CBOpCode.SRL_iHL:
                    case CBOpCode.SWAP_iHL:
                        return Ops(CPUReg.HLI);
                }

            switch (Op)
            {
                case OpCode.ADC_A_A:
                case OpCode.ADD_A_A:
                case OpCode.LD_A_A:
                case OpCode.SBC_A_A:
                    return Ops(CPUReg.A, CPUReg.A);

                case OpCode.ADC_A_B:
                case OpCode.ADD_A_B:
                case OpCode.LD_A_B:
                case OpCode.SBC_A_B:
                    return Ops(CPUReg.A, CPUReg.B);

                case OpCode.ADC_A_C:
                case OpCode.ADD_A_C:
                case OpCode.LD_A_C:
                case OpCode.SBC_A_C:
                    return Ops(CPUReg.A, CPUReg.C);

                case OpCode.ADC_A_D:
                case OpCode.ADD_A_D:
                case OpCode.LD_A_D:
                case OpCode.SBC_A_D:
                    return Ops(CPUReg.A, CPUReg.D);

                case OpCode.ADC_A_d8:
                case OpCode.ADD_A_d8:
                case OpCode.LD_A_d8:
                case OpCode.SBC_A_d8:
                    return Ops(CPUReg.A, OpD8);

                case OpCode.ADC_A_E:
                case OpCode.ADD_A_E:
                case OpCode.LD_A_E:
                case OpCode.SBC_A_E:
                    return Ops(CPUReg.A, CPUReg.E);

                case OpCode.ADC_A_H:
                case OpCode.ADD_A_H:
                case OpCode.LD_A_H:
                case OpCode.SBC_A_H:
                    return Ops(CPUReg.A, CPUReg.H);

                case OpCode.LD_A_iBC: return Ops(CPUReg.A, CPUReg.BCI);
                case OpCode.LD_A_iDE: return Ops(CPUReg.A, CPUReg.DEI);
                case OpCode.LD_A_iHLm: return Ops(CPUReg.A, CPUReg.HLIM);
                case OpCode.LD_A_iHLp: return Ops(CPUReg.A, CPUReg.HLIP);
                case OpCode.LD_A_a16: return Ops(CPUReg.A, R(OpA16));
                case OpCode.LD_a16_A: return Ops(W(OpA16), CPUReg.A);

                case OpCode.ADC_A_iHL:
                case OpCode.ADD_A_iHL:
                case OpCode.LD_A_iHL:
                case OpCode.SBC_A_iHL:
                    return Ops(CPUReg.A, CPUReg.HLI);

                case OpCode.ADC_A_L:
                case OpCode.ADD_A_L:
                case OpCode.LD_A_L:
                case OpCode.SBC_A_L:
                    return Ops(CPUReg.A, CPUReg.L);

                case OpCode.ADD_HL_BC: return Ops(CPUReg.HL, CPUReg.BC);
                case OpCode.ADD_HL_DE: return Ops(CPUReg.HL, CPUReg.DE);
                case OpCode.ADD_HL_HL: return Ops(CPUReg.HL, CPUReg.HL);
                case OpCode.ADD_HL_SP: return Ops(CPUReg.HL, CPUReg.SP);
                case OpCode.ADD_SP_r8: return Ops(CPUReg.SP, OpR8);

                case OpCode.AND_A:
                case OpCode.CP_A:
                case OpCode.DEC_A:
                case OpCode.INC_A:
                case OpCode.OR_A:
                case OpCode.SUB_A:
                case OpCode.XOR_A:
                    return Ops(CPUReg.A);

                case OpCode.AND_B:
                case OpCode.CP_B:
                case OpCode.DEC_B:
                case OpCode.INC_B:
                case OpCode.OR_B:
                case OpCode.SUB_B:
                case OpCode.XOR_B:
                    return Ops(CPUReg.B);

                case OpCode.AND_C:
                case OpCode.CP_C:
                case OpCode.DEC_C:
                case OpCode.INC_C:
                case OpCode.OR_C:
                case OpCode.SUB_C:
                case OpCode.XOR_C:
                    return Ops(CPUReg.C);

                case OpCode.AND_D:
                case OpCode.CP_D:
                case OpCode.DEC_D:
                case OpCode.INC_D:
                case OpCode.OR_D:
                case OpCode.SUB_D:
                case OpCode.XOR_D:
                    return Ops(CPUReg.D);

                case OpCode.AND_E:
                case OpCode.CP_E:
                case OpCode.DEC_E:
                case OpCode.INC_E:
                case OpCode.OR_E:
                case OpCode.SUB_E:
                case OpCode.XOR_E:
                    return Ops(CPUReg.E);

                case OpCode.AND_H:
                case OpCode.CP_H:
                case OpCode.DEC_H:
                case OpCode.INC_H:
                case OpCode.OR_H:
                case OpCode.SUB_H:
                case OpCode.XOR_H:
                    return Ops(CPUReg.H);

                case OpCode.AND_L:
                case OpCode.CP_L:
                case OpCode.DEC_L:
                case OpCode.INC_L:
                case OpCode.OR_L:
                case OpCode.SUB_L:
                case OpCode.XOR_L:
                    return Ops(CPUReg.L);

                case OpCode.AND_d8:
                case OpCode.CP_d8:
                case OpCode.OR_d8:
                case OpCode.SUB_d8:
                case OpCode.XOR_d8:
                    return Ops(OpD8);

                case OpCode.PUSH_AF:
                case OpCode.POP_AF:
                    return Ops(CPUReg.AF);

                case OpCode.DEC_BC:
                case OpCode.INC_BC:
                case OpCode.POP_BC:
                case OpCode.PUSH_BC:
                    return Ops(CPUReg.BC);

                case OpCode.DEC_DE:
                case OpCode.INC_DE:
                case OpCode.POP_DE:
                case OpCode.PUSH_DE:
                    return Ops(CPUReg.DE);

                case OpCode.DEC_HL:
                case OpCode.INC_HL:
                case OpCode.POP_HL:
                case OpCode.PUSH_HL:
                    return Ops(CPUReg.HL);

                case OpCode.AND_iHL:
                case OpCode.CP_iHL:
                case OpCode.DEC_iHL:
                case OpCode.INC_iHL:
                case OpCode.JP_iHL:
                case OpCode.OR_iHL:
                case OpCode.SUB_iHL:
                case OpCode.XOR_iHL:
                    return Ops(CPUReg.HLI);

                case OpCode.DEC_SP:
                case OpCode.INC_SP:
                    return Ops(CPUReg.SP);

                case OpCode.RST_00: return Ops(OpA(0x00));
                case OpCode.RST_08: return Ops(OpA(0x08));
                case OpCode.RST_10: return Ops(OpA(0x10));
                case OpCode.RST_18: return Ops(OpA(0x18));
                case OpCode.RST_20: return Ops(OpA(0x20));
                case OpCode.RST_28: return Ops(OpA(0x28));
                case OpCode.RST_30: return Ops(OpA(0x30));
                case OpCode.RST_38: return Ops(OpA(0x38));

                case OpCode.CALL_a16:
                case OpCode.JP_a16:
                    return Ops(OpJ16);

                case OpCode.CALL_C_a16:
                case OpCode.JP_C_a16:
                    return Ops(CPUFlag.C, OpJ16);

                case OpCode.CALL_NC_a16:
                case OpCode.JP_NC_a16:
                    return Ops(CPUFlag.NC, OpJ16);

                case OpCode.CALL_NZ_a16:
                case OpCode.JP_NZ_a16:
                    return Ops(CPUFlag.NZ, OpJ16);

                case OpCode.CALL_Z_a16:
                case OpCode.JP_Z_a16:
                    return Ops(CPUFlag.Z, OpJ16);

                case OpCode.RET_C: return Ops(CPUFlag.C);
                case OpCode.RET_NC: return Ops(CPUFlag.NC);
                case OpCode.RET_NZ: return Ops(CPUFlag.NZ);
                case OpCode.RET_Z: return Ops(CPUFlag.Z);

                case OpCode.JR_r8: return Ops(OpR8);
                case OpCode.JR_C_r8: return Ops(CPUFlag.C, OpR8);
                case OpCode.JR_NC_r8: return Ops(CPUFlag.NC, OpR8);
                case OpCode.JR_NZ_r8: return Ops(CPUFlag.NZ, OpR8);
                case OpCode.JR_Z_r8: return Ops(CPUFlag.Z, OpR8);

                case OpCode.LD_A_h8: return Ops(CPUReg.A, R(OpH8));
                case OpCode.LD_h8_A: return Ops(W(OpH8), CPUReg.A);

                case OpCode.LD_A_iC: return Ops(CPUReg.A, CPUReg.CH);
                case OpCode.LD_iC_A: return Ops(CPUReg.CH, CPUReg.A);

                case OpCode.LD_B_A: return Ops(CPUReg.B, CPUReg.A);
                case OpCode.LD_B_B: return Ops(CPUReg.B, CPUReg.B);
                case OpCode.LD_B_C: return Ops(CPUReg.B, CPUReg.C);
                case OpCode.LD_B_D: return Ops(CPUReg.B, CPUReg.D);
                case OpCode.LD_B_d8: return Ops(CPUReg.B, OpD8);
                case OpCode.LD_B_E: return Ops(CPUReg.B, CPUReg.E);
                case OpCode.LD_B_H: return Ops(CPUReg.B, CPUReg.H);
                case OpCode.LD_B_iHL: return Ops(CPUReg.B, CPUReg.HLI);
                case OpCode.LD_B_L: return Ops(CPUReg.B, CPUReg.L);
                case OpCode.LD_C_A: return Ops(CPUReg.C, CPUReg.A);
                case OpCode.LD_C_B: return Ops(CPUReg.C, CPUReg.B);
                case OpCode.LD_C_C: return Ops(CPUReg.C, CPUReg.C);
                case OpCode.LD_C_D: return Ops(CPUReg.C, CPUReg.D);
                case OpCode.LD_C_d8: return Ops(CPUReg.C, OpD8);
                case OpCode.LD_C_E: return Ops(CPUReg.C, CPUReg.E);
                case OpCode.LD_C_H: return Ops(CPUReg.C, CPUReg.H);
                case OpCode.LD_C_iHL: return Ops(CPUReg.C, CPUReg.HLI);
                case OpCode.LD_C_L: return Ops(CPUReg.C, CPUReg.L);
                case OpCode.LD_D_A: return Ops(CPUReg.D, CPUReg.A);
                case OpCode.LD_D_B: return Ops(CPUReg.D, CPUReg.B);
                case OpCode.LD_D_C: return Ops(CPUReg.D, CPUReg.C);
                case OpCode.LD_D_D: return Ops(CPUReg.D, CPUReg.D);
                case OpCode.LD_D_d8: return Ops(CPUReg.D, OpD8);
                case OpCode.LD_D_E: return Ops(CPUReg.D, CPUReg.E);
                case OpCode.LD_D_H: return Ops(CPUReg.D, CPUReg.H);
                case OpCode.LD_D_iHL: return Ops(CPUReg.D, CPUReg.HLI);
                case OpCode.LD_D_L: return Ops(CPUReg.D, CPUReg.L);
                case OpCode.LD_E_A: return Ops(CPUReg.E, CPUReg.A);
                case OpCode.LD_E_B: return Ops(CPUReg.E, CPUReg.B);
                case OpCode.LD_E_C: return Ops(CPUReg.E, CPUReg.C);
                case OpCode.LD_E_D: return Ops(CPUReg.E, CPUReg.D);
                case OpCode.LD_E_d8: return Ops(CPUReg.E, OpD8);
                case OpCode.LD_E_E: return Ops(CPUReg.E, CPUReg.E);
                case OpCode.LD_E_H: return Ops(CPUReg.E, CPUReg.H);
                case OpCode.LD_E_iHL: return Ops(CPUReg.E, CPUReg.HLI);
                case OpCode.LD_E_L: return Ops(CPUReg.E, CPUReg.L);
                case OpCode.LD_H_A: return Ops(CPUReg.H, CPUReg.A);
                case OpCode.LD_H_B: return Ops(CPUReg.H, CPUReg.B);
                case OpCode.LD_H_C: return Ops(CPUReg.H, CPUReg.C);
                case OpCode.LD_H_D: return Ops(CPUReg.H, CPUReg.D);
                case OpCode.LD_H_d8: return Ops(CPUReg.H, OpD8);
                case OpCode.LD_H_E: return Ops(CPUReg.H, CPUReg.E);
                case OpCode.LD_H_H: return Ops(CPUReg.H, CPUReg.H);
                case OpCode.LD_H_iHL: return Ops(CPUReg.H, CPUReg.HLI);
                case OpCode.LD_H_L: return Ops(CPUReg.H, CPUReg.L);
                case OpCode.LD_HL_SPpr8: return Ops(CPUReg.HL, OpS8);
                case OpCode.LD_i16_SP: return Ops(OpAI16, CPUReg.SP);
                case OpCode.LD_iBC_A: return Ops(CPUReg.BCI, CPUReg.A);
                case OpCode.LD_iDE_A: return Ops(CPUReg.DEI, CPUReg.A);
                case OpCode.LD_iHL_A: return Ops(CPUReg.HLI, CPUReg.A);
                case OpCode.LD_iHL_B: return Ops(CPUReg.HLI, CPUReg.B);
                case OpCode.LD_iHL_C: return Ops(CPUReg.HLI, CPUReg.C);
                case OpCode.LD_iHL_D: return Ops(CPUReg.HLI, CPUReg.D);
                case OpCode.LD_iHL_d8: return Ops(CPUReg.HLI, OpD8);
                case OpCode.LD_iHL_E: return Ops(CPUReg.HLI, CPUReg.E);
                case OpCode.LD_iHL_H: return Ops(CPUReg.HLI, CPUReg.H);
                case OpCode.LD_iHL_L: return Ops(CPUReg.HLI, CPUReg.L);
                case OpCode.LD_iHLm_A: return Ops(CPUReg.HLIM, CPUReg.A);
                case OpCode.LD_iHLp_A: return Ops(CPUReg.HLIP, CPUReg.A);
                case OpCode.LD_L_A: return Ops(CPUReg.L, CPUReg.A);
                case OpCode.LD_L_B: return Ops(CPUReg.L, CPUReg.B);
                case OpCode.LD_L_C: return Ops(CPUReg.L, CPUReg.C);
                case OpCode.LD_L_D: return Ops(CPUReg.L, CPUReg.D);
                case OpCode.LD_L_d8: return Ops(CPUReg.L, OpD8);
                case OpCode.LD_L_E: return Ops(CPUReg.L, CPUReg.E);
                case OpCode.LD_L_H: return Ops(CPUReg.L, CPUReg.H);
                case OpCode.LD_L_iHL: return Ops(CPUReg.L, CPUReg.HLI);
                case OpCode.LD_L_L: return Ops(CPUReg.L, CPUReg.L);
                case OpCode.LD_SP_HL: return Ops(CPUReg.SP, CPUReg.HL);

                case OpCode.LD_BC_d16: return Ops(CPUReg.BC, R(OpD16));
                case OpCode.LD_DE_d16: return Ops(CPUReg.DE, R(OpD16));
                case OpCode.LD_HL_d16: return Ops(CPUReg.HL, R(OpD16));
                case OpCode.LD_SP_d16: return Ops(CPUReg.SP, R(OpD16));

                default: return Ops();
            }
        }

        private static IOperand[] Ops(params IOperand[] parts) => parts;
        private static Address OpA(uint i) => new Address(i);
        private static IOperand OpAI(uint i) => new IndirectAddress(i);
        private static IOperand OpB(byte b) => new ByteValue(b);
        private static IOperand OpJ(uint i) => i < 0x8000 ? (IOperand)new BankedAddress(i) : new Address(i);
        private static IOperand OpP(int n) => new Plain(n);
        private static IOperand OpS(byte b) => new StackOffset(b);
        private Address OpA16 => OpA(Op16);
        private IOperand OpAI16 => OpAI(Op16);
        private IOperand OpD8 => OpB(OperandBytes[0]);
        private Address OpD16 => OpA(Op16);
        private Address OpH8 => OpA((uint)0xFF00 + OperandBytes[0]);
        private IOperand OpJ16 => OpJ(Op16);
        private IOperand OpR8 => OpJ((uint)(Location + 2 + (sbyte)OperandBytes[0]));
        private IOperand OpS8 => OpS(OperandBytes[0]);
        private uint Op16 => BitConverter.ToUInt16(OperandBytes, 0);
        private static IOperand R(Address ao) => ao.SetRead();
        private static IOperand W(Address ao) => ao.SetWrite();
    }
}
