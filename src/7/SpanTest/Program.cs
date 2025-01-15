using System;
using System.Runtime.InteropServices;
using Common;

namespace SpanTest
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine(nameof(ManagedHeap_Test));
            ManagedHeap_Test();
            Console.WriteLine();

            Console.WriteLine(nameof(UnmanagedHeap_Test));
            UnmanagedHeap_Test();
            Console.WriteLine();

            Console.WriteLine(nameof(Stack_Test));
            Stack_Test();
            Console.WriteLine();
        }

        private static void ManagedHeap_Test()
        {
            var arr = new int[10] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0 };

            var span = new Span<int>(arr);

            foreach (var s in span)
            {
                Console.WriteLine(s);
            }

            unsafe
            {
                fixed (int* p = arr, pp = span)
                {
                    Print.Address((long)p);
                    Print.Address((long)pp);
                }
            }
        }

        private static void UnmanagedHeap_Test()
        {
            //in 16bit machine, alloc 4 byte memory
            var size = sizeof(int);
            var ptr = Marshal.AllocHGlobal(size);

            unsafe
            {
                //used by 2*size byte, memory overflow!!!
                var span = new Span<int>(ptr.ToPointer(), 2);

                span[0] = 1;

                span[1] = 2;
            }

            Console.WriteLine(Marshal.ReadInt32(ptr));
            Console.WriteLine(Marshal.ReadInt32(ptr, size));

            Marshal.FreeHGlobal(ptr);
        }

        private static void Stack_Test()
        {
            Span<int> span = stackalloc int[5];
            //span[5] = 10;//throw System.IndexOutOfRangeException

            unsafe
            {
                //stack 由高地址向低地址分配空间
                var i = stackalloc int[1];
                var arr = stackalloc int[4];

                var span2 = new Span<int>(arr, sizeof(int) * 5);

                fixed (int* p = span2, p5 = &span2[4])
                {
                    Print.Address((long)arr);
                    Print.Address((long)i);

                    Print.Address((long)p);
                    Print.Address((long)p5);
                }
            }
        }
    }
}
