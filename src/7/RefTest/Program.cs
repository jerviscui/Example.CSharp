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
    }
}
