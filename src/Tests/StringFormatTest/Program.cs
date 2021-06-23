using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Running;
using Common;

namespace StringFormatTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkTest>();

            //var watch = new Stopwatch();
            //watch.Restart();
            //for (int i = 0; i < 1_000_000; i++)
            //{
            //    UnBoxTest();
            //}
            //watch.Stop();
            //Print.Microsecond(watch);

            //watch.Restart();
            //for (int i = 0; i < 1_000_000; i++)
            //{
            //    BoxTest();
            //}
            //watch.Stop();
            //Print.Microsecond(watch);
            //// 47,992 us
            ////150,643 us

            //Console.WriteLine(BoxFormatTest());
            //Console.WriteLine(UnBoxFormatTest());

            //var watch = new Stopwatch();
            //watch.Restart();
            //for (int i = 0; i < 1_000_000; i++)
            //{
            //    UnBoxFormatTest();
            //}
            //watch.Stop();
            //Print.Microsecond(watch);

            //watch.Restart();
            //for (int i = 0; i < 1_000_000; i++)
            //{
            //    BoxFormatTest();
            //}
            //watch.Stop();
            //Print.Microsecond(watch);
            ////118,433 us
            ////277,884 us
        }

        public static string UnBoxTest()
        {
            int a = 1;

            //编译器提示 IDE0071 简化内插
            return $"string {a.ToString()}";

            //IL_0003: ldstr        "string "
            //IL_0008: ldloca.s     a
            //IL_000a: call         instance string [System.Runtime]System.Int32::ToString()
            //IL_000f: call         string [System.Runtime]System.String::Concat(string, string)
        }

        public static string BoxTest()
        {
            int a = 1;

            return $"string {a}";

            //IL_0003: ldstr        "string {0}"
            //IL_0008: ldloc.0      // a
            //IL_0009: box          [System.Runtime]System.Int32
            //IL_000e: call         string [System.Runtime]System.String::Format(string, object)
        }

        public static string BoxFormatTest()
        {
            int a = 1;
            return $"{a,10:##,###}";

            //IL_0002: ldstr        "{0,10:##,###}"
            //IL_0007: ldloc.0      // a
            //IL_0008: box          [System.Runtime]System.Int32
            //IL_000d: call         string [System.Runtime]System.String::Format(string, object)
        }

        public static string UnBoxFormatTest()
        {
            int a = 1;
            return $"{a.ToString(),10:##,###}";

            //IL_0002: ldstr        "{0,10:##,###}"
            //IL_0007: ldloca.s     a
            //IL_0009: call         instance string [System.Runtime]System.Int32::ToString()
            //IL_000e: call         string [System.Runtime]System.String::Format(string, object)
        }
    }

    [MemoryDiagnoser]
    public class BenchmarkTest
    {
        //[Benchmark]
        public void UnBoxTest()
        {
            Program.UnBoxTest();
        }

        //[Benchmark]
        public void BoxTest()
        {
            Program.BoxTest();
        }

        [Benchmark]
        public void UnBoxFormatTest()
        {
            Program.UnBoxFormatTest();
        }

        [Benchmark]
        public void BoxFormatTest()
        {
            Program.BoxFormatTest();
        }
    }
}
