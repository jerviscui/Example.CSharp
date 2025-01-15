using System;
using Common;

namespace FixedTest
{
    internal sealed class Program
    {
        private static unsafe void Main(string[] args)
        {
            var obj = new P();

            //无法获取托管类型(P)的地址和大小，或声明指向它的指针
            //P* pp = &obj; //CS8500

            fixed (int* p = &obj.X)
            {
                *p = 1;
                //p++; //p是只读的，不能修改
                int* p2 = p;
                p2++;
                *p2 = 10; //y=10

                Print.Address((long)p);
            }

            fixed (Pp* p = &obj.Pp)
            {
                p->X = 1;
                p->Y = 2;

                (*p).X = 1;
                (*p).Y = 2;

                Print.Address((long)p);
            }

            fixed (int* xp = &obj.Pp.X, yp = &obj.Pp.Y)
            {
                Print.Address((long)xp);
                Print.Address((long)yp);
            }

            Console.WriteLine(obj.X);
            Console.WriteLine(obj.Y);
            Console.WriteLine();

            Console.WriteLine(nameof(ArrayAndString_Test));
            ArrayAndString_Test();
            Console.WriteLine();

            Console.WriteLine(nameof(Span_Test));
            Span_Test();
            Console.WriteLine();

            Console.WriteLine(nameof(FixedSizeBuffers_Test));
            FixedSizeBuffers_Test();
        }

        private static unsafe void ArrayAndString_Test()
        {
            int[] arr = { 1, 2, 3, 4, 5 };
            string s = "test";

            fixed (int* p = &arr[0])
            {
                Print.Address((long)p);
            }

            fixed (int* p = arr)
            {
                Print.Address((long)p);
            }

            fixed (char* p = s)
            {
                Print.Address((long)p);
            }
        }

        private static unsafe void Span_Test()
        {
            var s = new Span<int>(new[] { 0, 2, 3 });

            fixed (int* p = s) //等价于(int* p = &s.GetPinnableReference())
            {
                *p = 1;
            }

            Console.WriteLine(string.Join(',', s.ToArray()));
        }

        private static unsafe void FixedSizeBuffers_Test()
        {
            var a1 = new Sb { Arr = new int[5] };
            var a2 = new Fsb();

            Print.Address((long)&a2);
            Print.Address((long)a2.Arr);
            Print.Address((long)&a2.Arr); //数组指针变量的地址
            Print.Address((long)&a2.X);

            //Console.WriteLine(sizeof(SB));//SB作为托管类型不能计算大小
            Console.WriteLine(sizeof(Fsb));
        }

        private class P
        {
            public Pp Pp;

            public int X;

            public int Y;
        }

        private struct Pp
        {
            public int X;

            public int Y;
        }

        private struct Sb
        {
            public int[] Arr;

            public int X;
        }

        private unsafe struct Fsb
        {
            public fixed int Arr[5];

            public int X;
        }
    }
}
