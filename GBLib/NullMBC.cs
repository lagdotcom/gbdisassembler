using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;

namespace Lag.GBLib
{
    class NullMBC : IMBC
    {
        public NullMBC() { }
        public NullMBC(Gameboy parent) { }

        public void Apply(Word op, byte value)
        {
            throw new NotImplementedException();
        }

        public bool Handles(Word op) => false;

        public string Identify(Word op) => "?";

        public IEnumerable<Segment> Segments(RomHeader h)
        {
            return new Segment[] { new Segment("ROM", 0, h.ROM, 0) };
        }
    }
}
