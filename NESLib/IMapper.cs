using Lag.DisassemblerLib;
using System.Collections.Generic;

namespace Lag.NESLib
{
    public interface IMapper : IPortHandler
    {
        IEnumerable<Segment> Segments(RomHeader h);
    }
}
