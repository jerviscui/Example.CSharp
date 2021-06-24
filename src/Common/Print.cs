using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

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
        //[SuppressMessage("ReSharper", "UseFormatSpecifierInInterpolation")]
        public static void Nanosecond(Stopwatch stopwatch, string title = "")
        {
            Console.WriteLine($"{title} {(stopwatch.ElapsedTicks * 100).ToString("##,###"),10} ns");
        }

        /// <summary>
        /// 打印微秒数
        /// </summary>
        public static void Microsecond(Stopwatch stopwatch, string title = "")
        {
            Console.WriteLine($"{title} {(stopwatch.ElapsedTicks /10).ToString("##,###"),10} us");
        }

        /// <summary>
        /// 打印毫秒数
        /// </summary>
        public static void Millisecond(Stopwatch stopwatch, string title = "")
        {
            Console.WriteLine($"{title} {stopwatch.ElapsedMilliseconds.ToString("##,###"),10} ms");
        }
    }
}
