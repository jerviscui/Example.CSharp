using Common;
using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace StackallocTest
{
    class Program
    {
        unsafe static void Main(string[] args)
        {
            Console.WriteLine(nameof(StackOverflow_Test));
            StackOverflow_Test();

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

            Span_Test();
            Console.WriteLine();
        }

        unsafe static void Span_Test()
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

        unsafe static void StackOverflow_Test()
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
    }
}