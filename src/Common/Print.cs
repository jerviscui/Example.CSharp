using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

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
            Console.WriteLine($"{title} {(stopwatch.ElapsedTicks / 10).ToString("##,###"),10} us");
        }

        /// <summary>
        /// 打印毫秒数
        /// </summary>
        public static void Millisecond(Stopwatch stopwatch, string title = "")
        {
            Console.WriteLine($"{title} {stopwatch.ElapsedMilliseconds.ToString("##,###"),10} ms");
        }

        /// <summary>
        /// Windows 下内存使用情况
        /// </summary>
        public static void MemoryInfo()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                var process = Process.GetCurrentProcess();
                var pf1 = new PerformanceCounter("Process", "Working Set - Private", process.ProcessName);
                var pf2 = new PerformanceCounter("Process", "Working Set", process.ProcessName);

                Console.WriteLine($"{process.ProcessName}:工作集(进程类)  {process.WorkingSet64 / 1024,12:N3} KB");
                Console.WriteLine($"{process.ProcessName}:工作集          {pf2.NextValue() / 1024,12:N3} KB");
                //私有工作集
                Console.WriteLine($"{process.ProcessName}:专用工作集      {pf1.NextValue() / 1024,12:N3} KB");
            }
        }
    }
}
