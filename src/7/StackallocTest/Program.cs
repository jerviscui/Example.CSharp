using Common;
using System;
using System.Runtime.CompilerServices;

namespace StackallocTest
{
    class Program
    {
        static void Main(string[] args)
        {
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
        }

        unsafe static void Span_Test()
        {
            var a = stackalloc byte[8];
            Print.Address((long)a);
            Print.Address((long)(a+4));

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
    }
}
