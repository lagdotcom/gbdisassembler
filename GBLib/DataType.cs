using System;

namespace GBLib
{
    [Flags]
    public enum DataType
    {
        ByteSize = 0,
        WordSize = 1,

        Address = 0x1000,
        ROM = 0,
        RAM = 0x2000,

        Signed = 0x8000,
    }
}
