using Common;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace FixedTest
{
    class Program
    {
        class P
        {
            public int x;
            public int y;

            public PP pp;
        }

        struct PP
        {
            public int x;
            public int y;
        }

        unsafe static void Main(string[] args)
        {
            var obj = new P();

            //无法获取托管类型(P)的地址和大小，或声明指向它的指针
            //fixed (int* p = &obj)
            //{
            //}

            fixed (int* p = &obj.x)
            {
                *p = 1;
                //p++; //p是只读的，不能修改
                int* p2 = p;
                p2++;
                *p2 = 10;//y=10

                Print.Address((long)p);
            }

            fixed (PP* p = &obj.pp)
            {
                p->x = 1;
                p->y = 2;

                (*p).x = 1;
                (*p).y = 2;

                Print.Address((long)p);
            }

            fixed (int* xp = &obj.pp.x, yp = &obj.pp.y)
            {
                Print.Address((long)xp);
                Print.Address((long)yp);
            }

            Console.WriteLine(obj.x);
            Console.WriteLine(obj.y);
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

        unsafe static void ArrayAndString_Test()
        {
            int[] arr = { 1, 2, 3, 4, 5 };
            string s = "test";

            //无法获取托管类型(P)的地址和大小，或声明指向它的指针
            //&arr;
            //&arr[0];
            //&s;

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

        unsafe static void Span_Test()
        {
            var s = new Span<int>(new int[] { 0, 2, 3 });

            fixed (int* p = s)
            {
                *p = 1;
            }

            //todo: how to print span?
            Console.WriteLine(string.Join(',', s.ToArray()));
        }

        struct SB
        {
            public int[] arr;

            public int x;
        }

        unsafe struct FSB
        {
            public fixed int arr[5];

            public int x;
        }

        unsafe static void FixedSizeBuffers_Test()
        {
            var a1 = new SB() { arr = new int[5] };
            var a2 = new FSB();

            Print.Address((long)&a2);
            Print.Address((long)a2.arr);
            Print.Address((long)&a2.arr);//数组指针变量的地址
            Print.Address((long)&a2.x);

            //Console.WriteLine(sizeof(SB));//SB作为托管类型不能计算大小
            Console.WriteLine(sizeof(FSB));
        }
    }
}
