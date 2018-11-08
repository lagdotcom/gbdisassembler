using System;

namespace GBLib
{
    public class LR35902
    {
        public LR35902(byte[] mem)
        {
            Memory = mem;
        }

        public byte[] Memory;

        public Instruction Decode(int loc)
        {
            OpCode? op = GetOpCode(Memory[loc]);
            if (op == null) return null;

            return new Instruction(this, op.Value, loc);
        }

        public static OpCode? GetOpCode(byte b)
        {
            if (Enum.IsDefined(typeof(OpCode), b)) return (OpCode)b;

            return null;
        }

        public static bool IsOpEnd(OpCode op)
        {
            switch (op)
            {
                case OpCode.JP_a16:
                case OpCode.JP_iHL:
                case OpCode.JR_r8:
                case OpCode.RET:
                case OpCode.RETI:
                    return true;

                default: return false;
            }
        }

        public static int GetOperandSize(OpCode op)
        {
            switch (op)
            {
                case OpCode.ADC_A_d8:
                case OpCode.ADD_A_d8:
                case OpCode.ADD_SP_r8:
                case OpCode.AND_d8:
                case OpCode.CP_d8:
                case OpCode.JR_C_r8:
                case OpCode.JR_NC_r8:
                case OpCode.JR_NZ_r8:
                case OpCode.JR_r8:
                case OpCode.JR_Z_r8:
                case OpCode.LD_h8_A:
                case OpCode.LD_A_h8:
                case OpCode.LD_A_d8:
                case OpCode.LD_B_d8:
                case OpCode.LD_C_d8:
                case OpCode.LD_D_d8:
                case OpCode.LD_E_d8:
                case OpCode.LD_HL_SPpr8:
                case OpCode.LD_H_d8:
                case OpCode.LD_iHL_d8:
                case OpCode.LD_L_d8:
                case OpCode.OR_d8:
                case OpCode.SBC_A_d8:
                case OpCode.SUB_d8:
                case OpCode.XOR_d8:
                    return 1;

                case OpCode.CALL_a16:
                case OpCode.CALL_C_a16:
                case OpCode.CALL_NC_a16:
                case OpCode.CALL_NZ_a16:
                case OpCode.CALL_Z_a16:
                case OpCode.JP_a16:
                case OpCode.JP_C_a16:
                case OpCode.JP_NC_a16:
                case OpCode.JP_NZ_a16:
                case OpCode.JP_Z_a16:
                case OpCode.LD_a16_A:
                case OpCode.LD_A_a16:
                case OpCode.LD_BC_d16:
                case OpCode.LD_DE_d16:
                case OpCode.LD_HL_d16:
                case OpCode.LD_i16_SP:
                case OpCode.LD_SP_d16:
                    return 2;

                // TODO
                case OpCode.PREFIX_CB:
                    return 1;

                default: return 0;
            }
        }

        public static int? GetJumpDestination(OpCode op, int current, byte[] v)
        {
            switch (op)
            {
                case OpCode.CALL_a16:
                case OpCode.CALL_C_a16:
                case OpCode.CALL_NC_a16:
                case OpCode.CALL_NZ_a16:
                case OpCode.CALL_Z_a16:
                case OpCode.JP_a16:
                case OpCode.JP_C_a16:
                case OpCode.JP_NC_a16:
                case OpCode.JP_NZ_a16:
                case OpCode.JP_Z_a16:
                    int loc = BitConverter.ToInt16(v, 0);
                    // TODO: ROM switching
                    if (loc >= 0x4000) return null;
                    return loc;

                case OpCode.RST_00: return 0x00;
                case OpCode.RST_08: return 0x08;
                case OpCode.RST_10: return 0x10;
                case OpCode.RST_18: return 0x18;
                case OpCode.RST_20: return 0x20;
                case OpCode.RST_28: return 0x28;
                case OpCode.RST_30: return 0x30;
                case OpCode.RST_38: return 0x38;

                case OpCode.JR_C_r8:
                case OpCode.JR_NC_r8:
                case OpCode.JR_NZ_r8:
                case OpCode.JR_r8:
                case OpCode.JR_Z_r8:
                    sbyte rel = (sbyte)v[0];
                    return current + 1 + rel;

                default: return null;
            }
        }
    }
}
