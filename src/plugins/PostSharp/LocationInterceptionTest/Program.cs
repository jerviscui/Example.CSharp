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

            //new
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var t = new PropTest("", null);
            }
            watch.Stop();
            Print.Microsecond(watch);
            
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var t = new PropTest2("", null);
            }
            watch.Stop();
            Print.Microsecond(watch);

            //new and set
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var t = new PropTest("", null);
                t.S = "";
            }
            watch.Stop();
            Print.Microsecond(watch);
            
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var t = new PropTest2("", null);
                t.S = "";
            }
            watch.Stop();
            Print.Microsecond(watch);


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
