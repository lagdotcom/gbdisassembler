using Lag.DisassemblerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lag.GBLib
{
    class GameboyLabeller : Labeller
    {
        protected override void StandardLabels()
        {
            Labels = new Dictionary<uint, string>()
            {
                { 0x00, "_RST00" },
                { 0x08, "_RST08" },
                { 0x10, "_RST10" },
                { 0x18, "_RST18" },
                { 0x20, "_RST20" },
                { 0x28, "_RST28" },
                { 0x30, "_RST30" },
                { 0x38, "_RST38" },
                { 0x40, "_VBI" },
                { 0x48, "_LCDCI" },
                { 0x50, "_TIMERI" },
                { 0x58, "_SERI" },
                { 0x60, "_JOYI" },
                { 0x100, "_ENTRY" },
                { 0x150, "_MAIN" },
            };
        }
    }
}
