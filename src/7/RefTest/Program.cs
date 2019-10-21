using Common;
using System;
using System.Threading.Tasks;

namespace RefTest
{
    class Program
    {
        struct A
        {
            public int x;
            public int y;
        }

        static void Main(string[] args)
        {
            var arr = new A() { x = 1, y = 2 };
            var arr2 = arr;

            ref var cp = ref arr;
            cp.x = 10;
            Console.WriteLine(arr.x);
            Console.WriteLine(arr2.x);

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

            Console.WriteLine(b1.n);

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

        class B
        {
            public int x;
            public string n;

            public ref int GetX()
            {
                return ref x;
            }

        }

        static void ClassRefVariable_Test()
        {
            var b = new B();

            var x = b.GetX();
            ref var refx = ref b.GetX();

            x = 10;
            refx = 15;

            Console.WriteLine(b.x);
        }

        static void RefParameter_Test(ref B refb, B b)//B** refb, B* b
        {
            Console.WriteLine(refb == b);

            b.x = 10;
            b.n = "10";

            refb.x = 20;
            refb.n = "20";
        }

        static unsafe void ValueParameter_Test(int a, ref int refa)
        {
            int* p = &a;
            Print.Address((long)p);

            fixed (int* pp = &refa)
            {
                Print.Address((long)pp);
            }
        }

        static unsafe void InValueParameter_Test(int a, in int refa)
        {
            //refa = 10;//refa 是只读变量

            fixed (int* pp = &refa)
            {
                Print.Address((long)pp);
            }
        }
    }
}
