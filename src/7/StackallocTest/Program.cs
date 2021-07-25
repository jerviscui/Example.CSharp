using Common;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StackallocTest
{
    class Program
    {
        static unsafe void Main(string[] args)
        {
            //Console.WriteLine(nameof(StackOverflow_Test));
            //StackOverflow_Test();

            unsafe
            {
                int* a = stackalloc int[10];

                Print.Address((long)a);
            }

            Span<int> aa = stackalloc int[10];

            unsafe
            {
                fixed (int* p = aa)
                {
                    Print.Address((long)p);
                }
            }

            Console.WriteLine();
            Span_Test();
            Console.WriteLine();

            Console.WriteLine(nameof(StructAlloc_Test));
            StructAlloc_Test();
        }

        static unsafe void Span_Test()
        {
            var a = stackalloc byte[8];
            Print.Address((long)a);
            Print.Address((long)(a + 4));

            Span<int> span = new Span<int>(a, 2);
            span[0] = 1;
            span[1] = 2;

            fixed (int* p = span)
            {
                Print.Address((long)p);

                var pp = p;
                Print.Address((long)(pp + 1));
                Print.Address((long)(pp + 2));
            }
        }

        static int i = 0;

        static unsafe void StackOverflow_Test()
        {
            const int len = 1024;//1k
            //run on my computer, the max stack size 1494k, more than will throw StackOverflowException
            const int max = 1494;

            while (i < max)
            {
                i++;
                var a = stackalloc byte[len];
                Console.WriteLine(i);
                Print.Address((long)a);
            }
        }

        //struct A<T> where T : unmanaged
        //{
        //    public int X;

        //    public int Y;
        //}

        static unsafe void StructAlloc_Test()
        {
            //C# 8.0 才支持非托管构造类型
            //var n = sizeof(A<int>);
            //Console.WriteLine(n);

            //Span<A<int>> arr = stackalloc A<int>[]
            //{
            //    new A<int>() { X = 1, Y = 1 },
            //    new A<int>() { X = 2, Y = 2 },
            //    new A<int>() { X = 3, Y = 3 }
            //};

            //fixed (A<int>* p = arr)
            //{
            //    var p1 = p + 1;

            //    Print.Address((long)p);
            //    Print.Address((long)p1);

            //    Console.WriteLine(p1->X);
            //}
        }
    }
}