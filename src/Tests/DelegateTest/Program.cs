using System;
using BenchmarkDotNet.Running;

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

            MethodDelegate.GenericMethod();
        }
    }
}
