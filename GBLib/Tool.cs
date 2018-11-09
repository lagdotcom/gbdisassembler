using System;
using System.Text;
using GBLib.Operand;

namespace GBLib
{
    public static class Tool
    {
        public static T[] Slice<T>(T[] source, uint start, uint end)
        {
            T[] dest = new T[end - start];
            Array.Copy(source, start, dest, 0, end - start);

            return dest;
        }

        public static string ToAscii(this byte[] source)
        {
            return Encoding.ASCII.GetString(source).TrimEnd('\0');
        }

        public static BankedAddress Banked(uint address) => new BankedAddress(address);
        
        public static uint ReBank(uint current, uint loc)
        {
            BankedAddress c = new BankedAddress(current);
            BankedAddress l = new BankedAddress(loc, c.Bank);

            return l.AbsoluteAddress.Value;
        }
    }
}
