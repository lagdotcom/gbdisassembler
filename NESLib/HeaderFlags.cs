using System;

namespace Lag.NESLib
{
    [Flags]
    public enum HeaderFlags
    {
        VerticalMirroring = 0b1,
        HasPrgRam = 0b10,
        HasTrainer = 0b100,
        FourScreenVram = 0b1000,

        VsUnisystem = 0b1000,
        PlayChoice10 = 0b10000,
        HeaderVersion2 = 0b100000,

        Pal = 0b1000000,
        Dual = 0b10000000,

        BusConflicts = 0b100000000,
    }
}