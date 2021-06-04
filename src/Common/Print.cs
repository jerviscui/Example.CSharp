using System;
using System.Diagnostics;

namespace Common
{
    public static class Print
    {
        public static void Address(long p)
        {
            Console.WriteLine($"0x{p:x}");
        }

        /// <summary>
        /// 打印纳秒数
        /// </summary>
        public static void Nanosecond(Stopwatch stopwatch, string title = "")
        {
            Console.WriteLine($"{title} {stopwatch.ElapsedTicks * 100,10:##,###} ns");
        }

        /// <summary>
        /// 打印微秒数
        /// </summary>
        public static void Microsecond(Stopwatch stopwatch, string title = "")
        {
            Console.WriteLine($"{title} {stopwatch.ElapsedTicks / 10,10:##,###} us");
        }

        /// <summary>
        /// 打印毫秒数
        /// </summary>
        public static void Millisecond(Stopwatch stopwatch, string title = "")
        {
            Console.WriteLine($"{title} {stopwatch.ElapsedMilliseconds,10:##,###} ms");
        }
    }
}
