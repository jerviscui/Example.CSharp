using System;
using System.Diagnostics;
using BenchmarkDotNet.Running;
using Common;

namespace DelegateTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //BenchmarkRunner.Run<BenchmarkTest>();

            //new DelegateTest().GetMehtodTest();
            //Console.WriteLine();
            //new DelegateTest().GetAndExecTest();

            //MethodDelegate.DelegatePerformanceTest();

            //MethodDelegate.StaticMethodTest();

            //MethodDelegate.ExpressionTest();

            //MethodDelegate.GenericMethod();

            var watch = new Stopwatch();
            var p = new PropTest();

            Console.WriteLine(p.PropProtectedSetTest());

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                p.PropSetTest();
            }
            watch.Stop();
            Print.Microsecond(watch);

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                p.PropCacheSetTest();
            }
            watch.Stop();
            Print.Microsecond(watch);

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                p.PropDelegateSetTest();
            }
            watch.Stop();
            Print.Microsecond(watch);
            //242,679 us
            //160,200 us
            // 15,583 us
        }
    }
}
