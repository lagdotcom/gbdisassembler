using System;
using System.IO;
using System.Linq;
using Lag.DisassemblerLib;
using Lag.GBLib;
using Lag.NESLib;

namespace Lag.Disassembler
{
    public static class Serializer
    {
        const string InstructionMarker = "INS:";
        const string NameMarker = "NAM:";
        const string LabelMarker = "LBL:";
        const string CommentMarker = "COM:";
        const string CustomOperandMarker = "COP:";
        const string ContextMarker = "CTX:";

        public static IProject LoadProject(string fileName)
        {
            IProject project;

            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            using (BinaryReader br = new BinaryReader(fs))
                project = ReadProject(br);

            return project;
        }

        public static void SaveProject(string fileName, IProject project)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            using (BinaryWriter bw = new BinaryWriter(fs))
                WriteProject(bw, project);
        }

        private static IProject ReadProject(BinaryReader br)
        {
            bool cont = true;
            IProject project;
            string marker = new string(br.ReadChars(16));

            switch (marker)
            {
                case Gameboy.ProjectMarker:
                    project = new Gameboy();
                    break;

                case Nes.ProjectMarker:
                    project = new Nes();
                    break;

                default:
                    throw new InvalidDataException($"Unknown project marker: ${marker}");
            }

            project.Filename = br.ReadString();
            project.AcquireROM();

            while (cont)
                cont = ReadChunk(br, project);

            return project;
        }

        private static uint ReadAddress(BinaryReader br)
        {
            // TODO!
            return br.ReadUInt32();
        }

        private static bool ReadChunk(BinaryReader br, IProject project)
        {
            int count, i;
            string marker = new string(br.ReadChars(4));

            switch (marker)
            {
                case "":
                    return false;

                case InstructionMarker:
                    count = br.ReadInt32();
                    for (i = 0; i < count; i++)
                    {
                        uint address = ReadAddress(br);
                        project.Instructions[address] = project.Parse(address);
                    }
                    return true;

                case NameMarker:
                    count = br.ReadInt32();
                    for (i = 0; i < count; i++)
                    {
                        uint address = ReadAddress(br);
                        string name = br.ReadString();
                        project.Namer.Names[address] = name;
                    }
                    return true;

                case LabelMarker:
                    count = br.ReadInt32();
                    for (i = 0; i < count; i++)
                    {
                        uint address = ReadAddress(br);
                        string label = br.ReadString();
                        project.Labeller.Labels[address] = label;
                    }
                    return true;

                case CommentMarker:
                    count = br.ReadInt32();
                    for (i = 0; i < count; i++)
                    {
                        uint address = ReadAddress(br);
                        string label = br.ReadString();
                        project.Comments[address] = label;
                    }
                    return true;

                case CustomOperandMarker:
                    count = br.ReadInt32();
                    for (i = 0; i < count; i++)
                    {
                        uint address = ReadAddress(br);
                        project.AddCustomOperand(address, ReadOperand(br, project));
                    }
                    return true;

                case ContextMarker:
                    count = br.ReadInt32();
                    for (i = 0; i < count; i++)
                    {
                        uint address = ReadAddress(br);
                        project.Contexts.Add(address, ReadContext(br));
                    }
                    return true;

                default:
                    throw new InvalidDataException($"Unknown chunk type: {marker}");
            }
        }

        private static Word ReadOperand(BinaryReader br, IProject project)
        {
            WordFlags flags = (WordFlags)br.ReadByte();
            uint offset = ReadAddress(br);
            Word word;

            if (flags.HasFlag(WordFlags.HasSeg)) word = project.FromAbsolute(offset);
            else word = new Word(offset);

            if (flags.HasFlag(WordFlags.Hex)) word.IsHex = true;
            if (flags.HasFlag(WordFlags.Indirect)) word.Indirect = true;
            if (flags.HasFlag(WordFlags.Read)) word.Read = true;
            if (flags.HasFlag(WordFlags.Write)) word.Write = true;

            // TODO: remove
            //if (flags.HasFlag(WordFlags.ROM)) br.ReadByte();

            return word;
        }

        private static InstructionContext ReadContext(BinaryReader br)
        {
            var ctx = new InstructionContext();
            uint count = br.ReadUInt32();
            for (var i = 0; i < count; i++)
            {
                string name = br.ReadString();
                uint value = br.ReadUInt32();
                ctx.Add(name, value);
            }

            return ctx;
        }

        private static void WriteProject(BinaryWriter bw, IProject project)
        {
            WriteStatic(bw, project.Marker);
            WriteString(bw, project.Filename);

            if (project.Instructions.Count > 0)
            {
                WriteStatic(bw, InstructionMarker);
                var nondata = project.Instructions.Where(pair => pair.Value.IsReal);
                bw.Write(nondata.Count());
                foreach (var pair in nondata.OrderBy(pair => pair.Key))
                    WriteInstruction(bw, pair.Key, pair.Value);
            }

            if (project.Namer.Names.Count > 0)
            {
                WriteStatic(bw, NameMarker);
                bw.Write(project.Namer.Names.Count);
                foreach (var pair in project.Namer.Names.OrderBy(pair => pair.Key))
                    WriteNamedAddress(bw, pair.Key, pair.Value);
            }

            if (project.Labeller.Labels.Count > 0)
            {
                WriteStatic(bw, LabelMarker);
                bw.Write(project.Labeller.Labels.Count);
                foreach (var pair in project.Labeller.Labels.OrderBy(pair => pair.Key))
                    WriteNamedAddress(bw, pair.Key, pair.Value);
            }

            if (project.Comments.Count > 0)
            {
                WriteStatic(bw, CommentMarker);
                bw.Write(project.Comments.Count);
                foreach (var pair in project.Comments.OrderBy(pair => pair.Key))
                    WriteNamedAddress(bw, pair.Key, pair.Value);
            }

            if (project.CustomOperands.Count > 0)
            {
                WriteStatic(bw, CustomOperandMarker);
                bw.Write(project.CustomOperands.Count);
                foreach (var pair in project.CustomOperands.OrderBy(pair => pair.Key))
                    WriteCustomOperand(bw, pair.Key, pair.Value);
            }

            if (project.Contexts.Count > 0)
            {
                WriteStatic(bw, ContextMarker);
                bw.Write(project.Contexts.Count);
                foreach (var pair in project.Contexts.OrderBy(pair => pair.Key))
                    WriteContext(bw, pair.Key, pair.Value);
            }
        }

        private static void WriteInstruction(BinaryWriter bw, uint address, IInstruction inst)
        {
            bw.Write(address);
        }

        private static void WriteNamedAddress(BinaryWriter bw, uint address, string name)
        {
            bw.Write(address);
            WriteString(bw, name);
        }

        private static void WriteCustomOperand(BinaryWriter bw, uint address, Word word)
        {
            WordFlags fl = 0;
            if (word.Indirect) fl |= WordFlags.Indirect;
            if (word.IsHex) fl |= WordFlags.Hex;
            if (word.RAM) fl |= WordFlags.RAM;
            if (word.Read) fl |= WordFlags.Read;
            if (word.ROM) fl |= WordFlags.ROM;
            if (word.Write) fl |= WordFlags.Write;
            if (word.Seg != null) fl |= WordFlags.HasSeg;

            bw.Write(address);
            bw.Write((byte)fl);
            bw.Write(word.Absolute);
        }

        private static void WriteContext(BinaryWriter bw, uint address, InstructionContext ctx)
        {
            bw.Write(address);
            bw.Write(ctx.Count);
            foreach (var pair in ctx)
            {
                bw.Write(pair.Key);
                bw.Write(pair.Value);
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

        [Flags]
        enum WordFlags : byte
        {
            RAM = 1 << 0,
            ROM = 1 << 1,
            Hex = 1 << 2,
            Read = 1 << 3,
            Write = 1 << 4,
            Indirect = 1 << 5,
            HasSeg = 1 << 6,
        }
    }
}
