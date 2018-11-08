﻿namespace GBLib
{
    public enum OpCode : byte
    {
        NOP = 0x00,
        LD_BC_d16,
        LD_iBC_A,
        INC_BC,
        INC_B,
        DEC_B,
        LD_B_d8,
        RLCA,
        LD_i16_SP,
        ADD_HL_BC,
        LD_A_iBC,
        DEC_BC,
        INC_C,
        DEC_C,
        LD_C_d8,
        RRCA,

        STOP,
        LD_DE_d16,
        LD_iDE_A,
        INC_DE,
        INC_D,
        DEC_D,
        LD_D_d8,
        RLA,
        JR_r8,
        ADD_HL_DE,
        LD_A_iDE,
        DEC_DE,
        INC_E,
        DEC_E,
        LD_E_d8,
        RRA,

        JR_NZ_r8,
        LD_HL_d16,
        LD_iHLp_A,
        INC_HL,
        INC_H,
        DEC_H,
        LD_H_d8,
        DAA,
        JR_Z_r8,
        ADD_HL_HL,
        LD_A_iHLp,
        DEC_HL,
        INC_L,
        DEC_L,
        LD_L_d8,
        CPL,

        JR_NC_r8,
        LD_SP_d16,
        LD_iHLm_A,
        INC_SP,
        INC_iHL,
        DEC_iHL,
        LD_iHL_d8,
        SCF,
        JR_C_r8,
        ADD_HL_SP,
        LD_A_iHLm,
        DEC_SP,
        INC_A,
        DEC_A,
        LD_A_d8,
        CCF,

        LD_B_B,
        LD_B_C,
        LD_B_D,
        LD_B_E,
        LD_B_H,
        LD_B_L,
        LD_B_iHL,
        LD_B_A,
        LD_C_B,
        LD_C_C,
        LD_C_D,
        LD_C_E,
        LD_C_H,
        LD_C_L,
        LD_C_iHL,
        LD_C_A,

        LD_D_B,
        LD_D_C,
        LD_D_D,
        LD_D_E,
        LD_D_H,
        LD_D_L,
        LD_D_iHL,
        LD_D_A,
        LD_E_B,
        LD_E_C,
        LD_E_D,
        LD_E_E,
        LD_E_H,
        LD_E_L,
        LD_E_iHL,
        LD_E_A,

        LD_H_B,
        LD_H_C,
        LD_H_D,
        LD_H_E,
        LD_H_H,
        LD_H_L,
        LD_H_iHL,
        LD_H_A,
        LD_L_B,
        LD_L_C,
        LD_L_D,
        LD_L_E,
        LD_L_H,
        LD_L_L,
        LD_L_iHL,
        LD_L_A,

        LD_iHL_B,
        LD_iHL_C,
        LD_iHL_D,
        LD_iHL_E,
        LD_iHL_H,
        LD_iHL_L,
        HALT,
        LD_iHL_A,
        LD_A_B,
        LD_A_C,
        LD_A_D,
        LD_A_E,
        LD_A_H,
        LD_A_L,
        LD_A_iHL,
        LD_A_A,

        ADD_A_B,
        ADD_A_C,
        ADD_A_D,
        ADD_A_E,
        ADD_A_H,
        ADD_A_L,
        ADD_A_iHL,
        ADD_A_A,
        ADC_A_B,
        ADC_A_C,
        ADC_A_D,
        ADC_A_E,
        ADC_A_H,
        ADC_A_L,
        ADC_A_iHL,
        ADC_A_A,

        SUB_B,
        SUB_C,
        SUB_D,
        SUB_E,
        SUB_H,
        SUB_L,
        SUB_iHL,
        SUB_A,
        SBC_A_B,
        SBC_A_C,
        SBC_A_D,
        SBC_A_E,
        SBC_A_H,
        SBC_A_L,
        SBC_A_iHL,
        SBC_A_A,

        AND_B,
        AND_C,
        AND_D,
        AND_E,
        AND_H,
        AND_L,
        AND_iHL,
        AND_A,
        XOR_B,
        XOR_C,
        XOR_D,
        XOR_E,
        XOR_H,
        XOR_L,
        XOR_iHL,
        XOR_A,

        OR_B,
        OR_C,
        OR_D,
        OR_E,
        OR_H,
        OR_L,
        OR_iHL,
        OR_A,
        CP_B,
        CP_C,
        CP_D,
        CP_E,
        CP_H,
        CP_L,
        CP_iHL,
        CP_A,

        RET_NZ,
        POP_BC,
        JP_NZ_a16,
        JP_a16,
        CALL_NZ_a16,
        PUSH_BC,
        ADD_A_d8,
        RST_00,
        RET_Z,
        RET,
        JP_Z_a16,
        PREFIX_CB,
        CALL_Z_a16,
        CALL_a16,
        ADC_A_d8,
        RST_08,

        RET_NC,
        POP_DE,
        JP_NC_a16,
        CALL_NC_a16 = 0xD4,
        PUSH_DE,
        SUB_d8,
        RST_10,
        RET_C,
        RETI,
        JP_C_a16,
        CALL_C_a16 = 0xDC,
        SBC_A_d8 = 0xDE,
        RST_18,

        LD_h8_A,
        POP_HL,
        LD_iC_A,
        PUSH_HL = 0xE5,
        AND_d8,
        RST_20,
        ADD_SP_r8,
        JP_iHL,
        LD_a16_A,
        XOR_d8 = 0xEE,
        RST_28,

        LD_A_h8,
        POP_AF,
        LD_A_iC,
        DI,
        PUSH_AF = 0xF5,
        OR_d8,
        RST_30,
        LD_HL_SPpr8,
        LD_SP_HL,
        LD_A_a16,
        EI,
        CP_d8 = 0xFE,
        RST_38,
    }
}
