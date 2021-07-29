using Common;
using System;
using System.Diagnostics;

namespace LocationInterceptionTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var watch = new Stopwatch();

            //new
            Console.WriteLine("\r\nnew weaved");
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var t = new PropTest("", null);
            }
            watch.Stop();
            Print.Microsecond(watch);

            Console.WriteLine("\r\nnew native");
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var t = new PropTest2("", null);
            }
            watch.Stop();
            Print.Microsecond(watch);

            //warm up
            //new PropTest("", "").S = "";

            //new and set
            Console.WriteLine("\r\nnew and set weaved");
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var t = new PropTest("", null);
                t.S = "";
            }
            watch.Stop();
            Print.Microsecond(watch);

            //warm up
            //new PropTest2("", "").S = "";

            Console.WriteLine("\r\nnew and set native");
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var t = new PropTest2("", null);
                t.S = "";
            }
            watch.Stop();
            Print.Microsecond(watch);

            //output:
            //new weaved
            //        517 us
            //new native
            //     10,812 us
            //new and set weaved
            //     82,639 us
            //new and set native
            //     10,429 us

            //watch.Restart();
            //var t1 = new PropTest("", null);
            //watch.Stop();
            //Print.Microsecond(watch);
            //watch.Restart();
            //t1.S = "";
            //watch.Stop();
            //Print.Microsecond(watch);
            //watch.Restart();
            //t1.S = "";
            //watch.Stop();
            //Print.Microsecond(watch);

            //Console.WriteLine();
            //watch.Restart();
            //var t2 = new PropTest2("", null);
            //watch.Stop();
            //Print.Microsecond(watch);

            //watch.Restart();
            //t2.S = "";
            //watch.Stop();
            //Print.Microsecond(watch);
            //watch.Restart();
            //t2.S = "";
            //watch.Stop();
            //Print.Microsecond(watch);
        }
    }
}
