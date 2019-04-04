﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;
using ExtendedXmlSerializer.Configuration;
using ExtendedXmlSerializer.ExtensionModel.Xml;

namespace GBLib
{
    public class Disassembler
    {
        public Disassembler()
        {
            Id = Guid.NewGuid();
            Labeller = new Labeller(this);
            Namer = new Namer(this);
            CPU = new LR35902(this);

            Ports = new List<IPortHandler>
            {
                Labeller,
                Namer,
            };

            foreach (Type portType in Assembly.GetExecutingAssembly().GetTypes().Where(a => typeof(IPort).IsAssignableFrom(a) && !a.IsAbstract))
            {
                IPort port = (IPort)Activator.CreateInstance(portType, this);
                Ports.Add(port);
            }

            Instructions = new Dictionary<uint, Instruction>();
        }

        public Disassembler(string filename) : this()
        {
            Filename = filename;
            AcquireROM();
        }

        public Guid Id;

        public string Filename;

        [XmlIgnore]
        public LR35902 CPU;

        [XmlIgnore]
        public RomHeader Header;

        [XmlIgnore]
        public List<IPortHandler> Ports;

        [XmlIgnore]
        public byte[] ROM;

        [XmlIgnore]
        public IPortHandler MBC;

        public Dictionary<uint, Instruction> Instructions;
        public Labeller Labeller;
        public Namer Namer;

        public void StandardAnalysis()
        {
            Analyse(0x00);
            Analyse(0x08);
            Analyse(0x10);
            Analyse(0x18);
            Analyse(0x20);
            Analyse(0x28);
            Analyse(0x30);
            Analyse(0x38);
            Analyse(0x40);
            Analyse(0x48);
            Analyse(0x50);
            Analyse(0x58);
            Analyse(0x60);
            Analyse(0x100);
        }

        public void Analyse(uint start)
        {
            Queue<uint> locations = new Queue<uint>();
            locations.Enqueue(start);

            while (locations.Count > 0)
            {
                uint bloc = locations.Dequeue();
                uint loc = bloc;

                while (true)
                {
                    if (Instructions.ContainsKey(loc)) break;

                    Instruction i = CPU.Decode(loc);
                    if (i == null) break;

                    Instructions[loc] = i;

                    if (i.JumpLocation != null)
                        locations.Enqueue(i.JumpLocation.Value);

                    if (i.IsEnd) break;
                    loc += i.TotalSize;
                }
            }
        }

        public static Disassembler Load(string filename)
        {
            Disassembler instance;

            using (var stream = File.OpenRead(filename))
                instance = GetXmlSerializer().Deserialize<Disassembler>(new XmlReaderSettings { IgnoreWhitespace = false }, stream);

            instance.AcquireROM();
            return instance;
        }

        public void Save(string filename)
        {
            using (var writer = XmlWriter.Create(filename, new XmlWriterSettings { Indent = true }))
                GetXmlSerializer().Serialize(writer, this);
        }

        public IPortHandler FindHandler(IOperand op)
        {
            return Ports.FirstOrDefault(h => h.Handles(op));
        }

        private IPortHandler GetMBC(MemoryBankController mbc)
        {
            switch (mbc)
            {
                case MemoryBankController.MBC2: return new MBC2(this);
                case MemoryBankController.MBC5: return new MBC5(this);
                default: throw new NotImplementedException();
            }
        }

        private static IExtendedXmlSerializer GetXmlSerializer()
        {
            return new ConfigurationContainer()
                .UseOptimizedNamespaces()
                .ConfigureType<Disassembler>().EnableReferences(d => d.Id)
                .Create();
        }

        private void AcquireROM()
        {
            using (var f = File.Open(Filename, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                ROM = new byte[f.Length];
                f.Read(ROM, 0, (int)f.Length);
            }

            Header = new RomHeader(Tool.Slice(ROM, 0x100, 0x150));
            MBC = GetMBC(Header.MBC);
            Ports.Insert(0, MBC);
        }
    }
}