using System;
using System.Text;

namespace Lag.DisassemblerLib
{
    public static class UsefulExtensions
    {
        public static T[] Slice<T>(this T[] source, uint start, uint end)
        {
            T[] dest = new T[end - start];
            Array.Copy(source, start, dest, 0, end - start);

            return dest;
        }

        public static string ToAscii(this byte[] source)
        {
            return Encoding.ASCII.GetString(source).TrimEnd('\0');
        }

        public static int Clamp(this int value, int min, int max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static uint Clamp(this uint value, uint min, uint max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }
}
