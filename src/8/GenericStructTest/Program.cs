using Common;
using System;

namespace GenericStructTest
{
    class Program
    {
        static void Main(string[] args)
        {
            StructAlloc_Test();
        }

        struct A<T> where T : unmanaged
        {
            public int X;

            public int Y;
        }

        static unsafe void StructAlloc_Test()
        {
            var n = sizeof(A<int>);
            Console.WriteLine(n);

            Span<A<int>> arr = stackalloc A<int>[]
            {
                new A<int>() { X = 1, Y = 1 },
                new A<int>() { X = 2, Y = 2 },
                new A<int>() { X = 3, Y = 3 }
            };

            fixed (A<int>* p = arr)
            {
                var p1 = p + 1;

                Print.Address((long)p);
                Print.Address((long)p1);

                Console.WriteLine(p1->X);
            }
        }

        class T
        {
            public string Name { get; set; }
        }

        struct B<T>
        {
            public T X;

            public T Y;
        }

        static unsafe void ReferenceStruct_Test()
        {
            //todo: complete
            //var n = sizeof(B<T>);
        }
    }
}
