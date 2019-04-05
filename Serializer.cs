using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GBLib;

namespace GBDisassembler
{
    public static class Serializer
    {
        const string ProjectMarker = "GBDPROJ@LR35902:";
        const string InstructionMarker = "INS:";
        const string NameMarker = "NAM:";
        const string LabelMarker = "LBL:";

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
        }

        private static void ReadStatic(BinaryReader br, string source)
        {
            string check = new string(br.ReadChars(source.Length));
            if (check != source) throw new FormatException($"Expected: {source}, got {check}");
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
                WriteName(bw, pair.Key, pair.Value);

            WriteStatic(bw, LabelMarker);
            bw.Write(project.Labeller.Labels.Count);
            foreach (var pair in project.Labeller.Labels)
                WriteLabel(bw, pair.Key, pair.Value);
        }

        private static void WriteInstruction(BinaryWriter bw, uint address, Instruction inst)
        {
            bw.Write(address);
        }

        private static void WriteName(BinaryWriter bw, uint address, string name)
        {
            bw.Write(address);
            WriteString(bw, name);
        }

        private static void WriteLabel(BinaryWriter bw, uint address, string label)
        {
            bw.Write(address);
            WriteString(bw, label);
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
