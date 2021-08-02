using System;
using Common;

namespace RefTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var arr = new A { X = 1, Y = 2 };
            var arr2 = arr;

            ref var cp = ref arr;
            cp.X = 10;
            Console.WriteLine(arr.X);
            Console.WriteLine(arr2.X);

            unsafe
            {
                fixed (A* p = &cp)
                {
                    Print.Address((long)p);
                    Print.Address((long)&arr);
                    Print.Address((long)&arr2);
                }
            }

            ClassRefVariable_Test();

            Console.WriteLine();
            var b1 = new B();

            RefParameter_Test(ref b1, b1);

            Console.WriteLine(b1.N);

            Console.WriteLine();
            int a = 1;
            int aa = 2;
            unsafe
            {
                Print.Address((long)&a);
                Print.Address((long)&aa);
            }
            ValueParameter_Test(a, ref aa);

            Console.WriteLine();
            unsafe
            {
                Print.Address((long)&aa);
            }
            InValueParameter_Test(a, aa);
        }

        private static void ClassRefVariable_Test()
        {
            var b = new B();

            var x = b.GetX();
            ref var refx = ref b.GetX();

            x = 10;
            refx = 15;

            Console.WriteLine(b.X);
        }

        private static void RefParameter_Test(ref B refb, B b) //B** refb, B* b
        {
            Console.WriteLine(refb == b);

            b.X = 10;
            b.N = "10";

            refb.X = 20;
            refb.N = "20";
        }

        private static unsafe void ValueParameter_Test(int a, ref int refa)
        {
            int* p = &a;
            Print.Address((long)p);

            fixed (int* pp = &refa)
            {
                Print.Address((long)pp);
            }
        }

        private static unsafe void InValueParameter_Test(int a, in int refa)
        {
            //refa = 10;//refa 是只读变量

            fixed (int* pp = &refa)
            {
                Print.Address((long)pp);
            }
        }

        private struct A
        {
            public int X;

            public int Y;
        }

        private class B
        {
            public string N;

            public int X;

            public ref int GetX()
            {
                return ref X;
            }
        }
    }
}
