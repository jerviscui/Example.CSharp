using System;
using System.Diagnostics;
using Common;

namespace LocationInterceptionTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello World!");

            var watch = new Stopwatch();

            watch.Start();
            var t = new PropTest("", null);
            watch.Stop();
            Print.Microsecond(watch);

            watch.Restart();
            t.S = "";
            watch.Stop();
            Print.Microsecond(watch);
            watch.Restart();
            t.S = "";
            watch.Stop();
            Print.Microsecond(watch);

            Console.WriteLine();
            watch.Restart();
            var t2 = new PropTest2("", null);
            watch.Stop();
            Print.Microsecond(watch);

            watch.Restart();
            t2.S = "";
            watch.Stop();
            Print.Microsecond(watch);
            watch.Restart();
            t2.S = "";
            watch.Stop();
            Print.Microsecond(watch);

            //Console.WriteLine($"S:{t.S}\r\nN:{t.N}");
        }
    }
}
