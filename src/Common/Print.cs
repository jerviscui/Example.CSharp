using System;

namespace Common
{
    public static class Print
    {
        public static void Address(long p)
        {
            Console.WriteLine($"0x{p:x}");
        }
    }
}
