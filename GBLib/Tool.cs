using System;
using System.Text;

namespace GBLib
{
    public static class Tool
    {
        public static T[] Slice<T>(T[] source, int start, int end)
        {
            T[] dest = new T[end - start];
            Array.Copy(source, start, dest, 0, end - start);

            return dest;
        }

        public static string ToAscii(this byte[] source)
        {
            return Encoding.ASCII.GetString(source).TrimEnd('\0');
        }
    }
}
