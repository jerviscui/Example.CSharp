using System;
using System.Collections.Generic;
using System.Text;

namespace FixedTest
{
    public static class Print
    {
        public static void Address(long p)
        {
            Console.WriteLine($"0x{p:x}");
        }
    }
}
