using System;
using System.IO;
using System.Linq;
using Lag.DisassemblerLib;
using Lag.GBLib;

namespace Lag.Disassembler
{
    class Program
    {
        static void Main(string[] args)
        {
            uint offset = 0;
            string filename;
            if (args.Length < 2)
            {
                if (args.Length < 1)
                {
                    Console.WriteLine("Syntax: exe romfile [offset in hex]");
                    return;
                }

                filename = args[0];
            }
            else
            {
                filename = args[0];

                offset = Convert.ToUInt32(args[1], 16);
            }

            byte[] raw;
            FileInfo fi = new FileInfo(filename);
            int size = (int)fi.Length;
            raw = new byte[size];

            Gameboy dis = new Gameboy(filename);
            if (offset > 0)
                dis.Analyse(offset);
            else
                dis.StandardAnalysis();

            dis.Namer.Names.Add(0xC002, "StartupA");
            dis.Namer.Names.Add(0xC048, "ROMBankBackup");
            dis.Namer.Names.Add(0xFF9D, "SVBKCopy");
            dis.Namer.Names.Add(0xFF9E, "ROMBankCopy");

            Console.WriteLine($"{dis.Header}\n");

            foreach (uint address in dis.Instructions.Keys.OrderBy(k => k))
            {
                IInstruction i = dis.Instructions[address];

                if (dis.Labeller.Labels.ContainsKey(address))
                    Console.WriteLine($"{dis.Labeller.Labels[address]}:");
                Console.WriteLine(i);
                if (i.IsEnd) Console.WriteLine("\n");
            }
        }
    }
}
