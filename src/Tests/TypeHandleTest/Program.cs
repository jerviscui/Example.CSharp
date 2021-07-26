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
            new TypeInfoTests().Type_GetProperties_Test();
            Console.WriteLine();
            new TypeInfoTests().Type_GetRuntimeProperties_Test();
            Console.WriteLine();
            new TypeInfoTests().TypeInfo_DeclaredProperties_Test();
            Console.WriteLine();

            //BenchmarkRunner.Run<BenchmarkTests>();

            //new Test().GetAttributes();
            //Console.WriteLine();
            //new Test().Handle();
            //Console.WriteLine();
            //new Test().GenericHandle();
            //Console.WriteLine();
            //new Test().ReferenceGenericHandle();

            //Console.WriteLine();
            //new HandleSizeTest().RuntimeHandleAndType();
        }
    }
}
