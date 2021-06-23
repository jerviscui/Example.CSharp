using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using BenchmarkDotNet.Running;

namespace TypeHandleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            BenchmarkRunner.Run<BenchmarkTests>();

            //Console.WriteLine("Hello World!");

            //new Test().GetAttributes();
            //Console.WriteLine();
            //new Test().Handle();
            //Console.WriteLine();
            //new Test().GenericHandle();
            //Console.WriteLine();
            //new Test().ReferenceGenericHandle();

            //Console.WriteLine();
            //new HandleSizeTest().RuntimeHandleAndType();

            //Console.ReadKey();
        }
    }
}
