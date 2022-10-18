using Lag.DisassemblerLib;
using System.Collections.Generic;

namespace Lag.GBLib
{
    public interface IMBC : IPortHandler
    {
        IEnumerable<Segment> Segments(RomHeader h);
    }
}
