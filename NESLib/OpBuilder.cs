using System;
using System.Collections.Generic;
using Lag.DisassemblerLib;

namespace Lag.NESLib
{
    internal class OpBuilder
    {
        private List<IOperand> ops;
        private byte[] bytes;
        private int offset;
        private Instruction inst;
        private Word last;

        public OpBuilder(Instruction instruction, byte[] raw)
        {
            bytes = raw;
            offset = 0;
            ops = new List<IOperand>();
            inst = instruction;
        }

        public OpBuilder Absolute()
        {
            return Add(NextWord());
        }

        public OpBuilder Immediate() => Add(NextByte());

        public OpBuilder Indirect(string index = null)
        {
            NesWord op = NextWord();

            if (!string.IsNullOrEmpty(index)) op.Index = index;
            op.Indirect = true;

            return Add(op);
        }

        public OpBuilder Relative()
        {
            Segment seg = inst.Project.GetSegment(inst.Location);
            sbyte offset = (sbyte)ReadByte();
            return Add(new Word(seg, (uint)(inst.Location + 2 + offset)));
        }

        public OpBuilder ZeroPage()
        {
            uint offset = ReadByte();
            return Add(new Word(offset));
        }

        public OpBuilder Register(string reg) => Add(new RegOp(reg));

        public IOperand[] Resolve() => ops.ToArray();

        public OpBuilder Index(string reg) => Add(new RegOp(reg));

        public OpBuilder IsRead()
        {
            last.Read = true;
            return this;
        }

        public OpBuilder IsWrite()
        {
            last.Write = true;
            return this;
        }

        private OpBuilder Add(IOperand op)
        {
            last = op as Word;
            ops.Add(op);
            return this;
        }

        private IOperand NextByte() => new DisassemblerLib.Byte(ReadByte()) { IsHex = true };

        private NesWord NextWord()
        {
            Segment seg = inst.Project.GetSegment(inst.Location);
            uint value = ReadWord();

            if (seg != null && seg.RAMContains(value)) return new NesWord(seg, value);
            return new NesWord(value);
        }

        private uint ReadWord()
        {
            byte lo = ReadByte();
            byte hi = ReadByte();

            return (uint)(lo + (hi << 8));
        }

        private byte ReadByte() => bytes[offset++];
    }
}