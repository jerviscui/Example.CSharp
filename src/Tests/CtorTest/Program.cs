using System;
using System.Diagnostics;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Common;

namespace DelegateTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkTest>();

            //new A("p1");
            //new B("p1");

            //var watch = new Stopwatch();
            //watch.Restart();
            //for (int i = 0; i < 1_000_000; i++)
            //{
            //    var a = new A("p1");
            //}
            //watch.Stop();
            //Print.Microsecond(watch);
            //watch.Restart();
            //for (int i = 0; i < 1_000_000; i++)
            //{
            //    var a = new B("p1");
            //}
            //watch.Stop();
            //Print.Microsecond(watch);

            //   311 us
            //19,236 us
        }
    }

    public class A
    {
        public string P1 { get; set; }

        public string P2 { get; set; }

        public string P3 { get; set; }

        public string P4 { get; set; }

        public A(string p1)
        {
            P1 = p1;
        }
    }

    public class B
    {
        //赋值为 null 不会有性能影响，string.Empty 会额外生成赋值 IL
        public string P1 { get; set; } = null!;

        public string P2 { get; set; } = null!;

        public string P3 { get; set; } = null!;

        public string P4 { get; set; } = null!;

        public B(string p1)
        {
            P1 = p1;
        }
    }

    [MemoryDiagnoser]
    public class BenchmarkTest
    {
        [Benchmark]
        public void NoDefaultTest()
        {
            var a = new A("p1");
        }

        [Benchmark]
        public void DefaultTest()
        {
            var a = new B("p1");
        }
    }
}
