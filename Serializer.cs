using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBLib;
using GBLib.Operand;

namespace GBDisassembler
{
    public static class Serializer
    {
        const string ProjectMarker = "GBDPROJ@LR35902:";
        const string InstructionMarker = "INS:";
        const string NameMarker = "NAM:";
        const string LabelMarker = "LBL:";
        const string CommentMarker = "COM:";
        const string CustomOperandMarker = "COP:";

        public static Disassembler LoadProject(string fileName)
        {
            Disassembler project = new Disassembler();

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
                ReadProject(br, project);

            return project;
        }

        public static void SaveProject(string fileName, Disassembler project)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
                WriteProject(bw, project);
        }

        private static void ReadProject(BinaryReader br, Disassembler project)
        {
            int count, i;

            ReadStatic(br, ProjectMarker);
            project.Filename = br.ReadString();
            project.AcquireROM();

            ReadStatic(br, InstructionMarker);
            count = br.ReadInt32();
            for (i = 0; i < count; i++)
            {
                uint address = br.ReadUInt32();
                project.Instructions[address] = new Instruction(project, (OpCode)project.ROM[address], address);
            }

            ReadStatic(br, NameMarker);
            count = br.ReadInt32();
            for (i = 0; i < count; i++)
            {
                uint address = br.ReadUInt32();
                string name = br.ReadString();
                project.Namer.Names[address] = name;
            }

            ReadStatic(br, LabelMarker);
            count = br.ReadInt32();
            for (i = 0; i < count; i++)
            {
                uint address = br.ReadUInt32();
                string label = br.ReadString();
                project.Labeller.Labels[address] = label;
            }

            ReadStatic(br, CommentMarker);
            count = br.ReadInt32();
            for (i = 0; i < count; i++)
            {
                uint address = br.ReadUInt32();
                string label = br.ReadString();
                project.Comments[address] = label;
            }

            ReadStatic(br, CustomOperandMarker);
            count = br.ReadInt32();
            for (i = 0; i < count; i++)
            {
                uint address = br.ReadUInt32();
                int copCount = br.ReadInt32();
                for (int j = 0; j < copCount; j++)
                {
                    int index = br.ReadInt32();
                    project.AddCustomOperand(address, index, ReadOperand(br));
                }
            }
        }

        private static void ReadStatic(BinaryReader br, string source)
        {
            string check = new string(br.ReadChars(source.Length));
            if (check != source) throw new FormatException($"Expected: {source}, got {check}");
        }

        private static IOperand ReadOperand(BinaryReader br)
        {
            char key = br.ReadChar();

            switch (key)
            {
                case 'A': return new Address(br.ReadUInt32());
                case 'B': return new BankedAddress(br.ReadUInt32());
                case 'b': return new ByteValue((byte)br.ReadUInt32());
                case 'I': return new IndirectAddress(br.ReadUInt32());
                case 'p': return new Plain(br.ReadUInt32());
                case 's': return new StackOffset((byte)br.ReadUInt32());
                case 'w': return new WordValue(br.ReadUInt32());
                default: throw new InvalidDataException($"Cannot read Operand of type {key}");
            }
        }

        private static void WriteProject(BinaryWriter bw, Disassembler project)
        {
            WriteStatic(bw, ProjectMarker);
            WriteString(bw, project.Filename);

            WriteStatic(bw, InstructionMarker);
            bw.Write(project.Instructions.Count);
            foreach (var pair in project.Instructions)
                WriteInstruction(bw, pair.Key, pair.Value);

            WriteStatic(bw, NameMarker);
            bw.Write(project.Namer.Names.Count);
            foreach (var pair in project.Namer.Names)
                WriteNamedAddress(bw, pair.Key, pair.Value);

            WriteStatic(bw, LabelMarker);
            bw.Write(project.Labeller.Labels.Count);
            foreach (var pair in project.Labeller.Labels)
                WriteNamedAddress(bw, pair.Key, pair.Value);

            WriteStatic(bw, CommentMarker);
            bw.Write(project.Comments.Count);
            foreach (var pair in project.Comments)
                WriteNamedAddress(bw, pair.Key, pair.Value);

            WriteStatic(bw, CustomOperandMarker);
            bw.Write(project.CustomOperands.Count);
            foreach (var pair in project.CustomOperands)
                WriteCustomOperands(bw, pair.Key, pair.Value);
        }

        private static void WriteInstruction(BinaryWriter bw, uint address, Instruction inst)
        {
            bw.Write(address);
        }

        private static void WriteNamedAddress(BinaryWriter bw, uint address, string name)
        {
            bw.Write(address);
            WriteString(bw, name);
        }

        private static void WriteCustomOperands(BinaryWriter bw, uint address, CustomOperandList ops)
        {
            bw.Write(address);
            bw.Write(ops.Count);
            foreach (var pair in ops)
            {
                bw.Write(pair.Key);
                bw.Write(pair.Value.TypeKey);
                if (pair.Value.TypeValue.HasValue) bw.Write(pair.Value.TypeValue.Value);
            }
        }

        private static void WriteStatic(BinaryWriter bw, string source)
        {
            bw.Write(source.ToCharArray());
        }

        private static void WriteString(BinaryWriter bw, string source)
        {
            bw.Write(string.IsNullOrWhiteSpace(source) ? string.Empty : source);
        }
    }
}
