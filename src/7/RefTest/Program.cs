using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Common;

namespace RefTest
{
    internal sealed class Program
    {
        private static unsafe void Struct_Pointer_Test()
        {
            var arr = new A { X = 1, Y = 2 };

            ref A a = ref arr;

            //fixed (A* p = &arr) { }//CS2013, arr is fixed
            A* pp = &arr;

            fixed (A* p = &a) //p = ldloca.s arr
            {
                Print.Address((long)p);
                Print.Address((long)&arr);
            }
        }

        private static unsafe void Struct_ValueCopy_Test()
        {
            var arr = new A { X = 1, Y = 2 };
            var arr2 = arr; //struct copy

            ref var cp = ref arr;
            cp.X = 10;
            Console.WriteLine(arr.X);
            Console.WriteLine(arr2.X);

            fixed (A* p = &cp)
            {
                Print.Address((long)p);
                Print.Address((long)&arr);
                Print.Address((long)&arr2);
            }
        }

        private static unsafe void Int_Pointer_Test()
        {
            int x = 10;

            ref int a = ref x;

            fixed (int* p = &a)
            {
                Print.Address((long)p);
                Print.Address((long)&x);
            }
        }

        private static unsafe void Class_Pointer_Test()
        {
            var b = new B { N = "test", X = 1 };

            //var v = &b; //CS8500 不能获取托管类型的指针
            //ref var r = ref b;
            //fixed (B* p = &r) //CS8500
            //{
            //    Print.Address((long)p);
            //    Print.Address((long)&r); //CS8500
            //}

            //class pointer
            var up = Unsafe.AsPointer(ref b);
            var ptr = new IntPtr(up);
            Print.Address(ptr.ToInt64());

            var copy = Unsafe.AsRef<B>(up);
            Console.WriteLine("B.N = " + copy.N);
        }

        private static unsafe void Class_Pointer_GCHandle_Test()
        {
            var b = new B { N = "test", X = 1 };

            //class pointer
            GCHandle objHandle = GCHandle.Alloc(b, GCHandleType.WeakTrackResurrection);
            var ptr = GCHandle.ToIntPtr(objHandle);
            Print.Address(ptr.ToInt64());
            //objHandle.Target;
            object obj = GCHandle.FromIntPtr(ptr).Target;
            Console.WriteLine("B.N = " + ((B)obj).N);
            objHandle.Free();

            //field pointer
            fixed (int* p = &b.X)
            {
                Print.Address((long)p); // 打印内存地址
                Console.WriteLine(*p);  // 打印值
            }
        }

        private static void Main(string[] args)
        {
            Struct_Pointer_Test();
            Console.WriteLine();

            Int_Pointer_Test();
            Console.WriteLine();

            Struct_ValueCopy_Test();
            Console.WriteLine();

            Class_Pointer_Test();
            Console.WriteLine();

            Class_Pointer_GCHandle_Test();
            Console.WriteLine();

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
            ValueParameter_Address_Test(a, ref aa);
            Console.WriteLine();
        }

        private static void ClassRefVariable_Test()
        {
            var b = new B();

            var x = b.GetX(); // value copy
            ref var refx = ref b.GetX();

            x = 10;
            refx = 15;

            Console.WriteLine(b.X); //15
        }

        //b is a pointer to the heap
        //refb is a pointer to pointer
        private static void RefParameter_Test(ref B refb, B b) //B* refb, B b
        {
            Console.WriteLine(refb == b);

            b.X = 10;
            b.N = "10";

            refb.X = 20;
            refb.N = "20";
        }

        private static unsafe void ValueParameter_Address_Test(int a, ref int refa)
        {
            int* p = &a;
            Print.Address((long)p);

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
