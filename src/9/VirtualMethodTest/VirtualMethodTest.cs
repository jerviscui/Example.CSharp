using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace VirtualMethodTest
{
    public class VirtualMethodTest
    {
        private class MyBaseClass : IMyInterface
        {
            //[MethodImpl(MethodImplOptions.AggressiveInlining)]
            public virtual void MyFunc()
            {
                int i = 1;
                int j = i++;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public virtual void EmptyFunc()
            {
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public virtual void NoInlineEmptyFunc()
            {
            }
        }

        private interface IMyInterface
        {
            void MyFunc();

            void EmptyFunc();

            void NoInlineEmptyFunc();
        }

        private class MyClass : MyBaseClass
        {
            //[MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override void MyFunc()
            {
                int i = 2;
                int j = i++;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public override void EmptyFunc()
            {
            }

            [MethodImpl(MethodImplOptions.NoInlining)]
            public override void NoInlineEmptyFunc()
            {
            }
        }

        public void OverrideMethodTest()
        {
            Console.WriteLine("OverrideMethodTest");
            Stopwatch stopwatch = new();

            //base excute
            MyBaseClass myBaseClass = new();
            myBaseClass.MyFunc();
            stopwatch.Start();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                myBaseClass.MyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"base excute: {stopwatch.ElapsedMilliseconds}");

            //child excute override
            MyClass myClass = new();
            myClass.MyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                myClass.MyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"child excute: {stopwatch.ElapsedMilliseconds}");

            //没有差别
            //OverrideEmptyMethodTest
            //base excute: 297
            //child excute: 306
        }

        public void OverrideEmptyMethodTest()
        {
            Console.WriteLine("OverrideEmptyMethodTest");
            Stopwatch stopwatch = new();

            //base excute
            MyBaseClass myBaseClass = new();
            myBaseClass.EmptyFunc();
            stopwatch.Start();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                myBaseClass.EmptyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"base excute: {stopwatch.ElapsedMilliseconds}");

            //child excute override
            MyClass myClass = new();
            myClass.EmptyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                myClass.EmptyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"child excute: {stopwatch.ElapsedMilliseconds}");

            //没有差别
            //OverrideMethodTest
            //base excute: 316
            //child excute: 315
        }

        public void CovariantExecMethodTest()
        {
            Console.WriteLine("CovariantExecMethodTest");
            Stopwatch stopwatch = new();

            //child excute override
            MyClass myClass = new();
            myClass.MyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                myClass.MyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"child excute: {stopwatch.ElapsedMilliseconds}");

            //covariant excute override
            MyBaseClass contravariant = new MyClass();
            contravariant.MyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                contravariant.MyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"covariant excute: {stopwatch.ElapsedMilliseconds}");
        }

        public void ChangeRuntimeTypeAndCovariantExecMethodTest()
        {
            Console.WriteLine("ChangeRuntimeTypeAndCovariantExecMethodTest");
            Stopwatch stopwatch = new();

            //child excute override
            MyClass myClass = new();
            myClass.MyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                myClass.MyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"child excute: {stopwatch.ElapsedMilliseconds}");

            //covariant excute override
            MyBaseClass contravariant = new();
            contravariant = new MyClass(); //多次赋值以使JIT编译器无法确认该变量的运行时类型
            contravariant.MyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                contravariant.MyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"covariant excute: {stopwatch.ElapsedMilliseconds}");

            //性能差距6倍，原因是最后的 MyFunc 没有在运行时进行方法内联优化
            //ChangeRuntimeTypeAndCovariantExecMethodTest
            //child excute: 290
            //covariant excute: 1820
        }

        public void NoInlineTest()
        {
            Console.WriteLine("NoInlineTest");
            Stopwatch stopwatch = new();

            //child excute override
            MyClass myClass = new();
            myClass.EmptyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                myClass.EmptyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"child excute inline(default): {stopwatch.ElapsedMilliseconds}");

            myClass.NoInlineEmptyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                myClass.NoInlineEmptyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"child excute no inline: {stopwatch.ElapsedMilliseconds}");

            //NoInlineTest，默认运行时中会进行方法内联优化
            //child excute inline(default): 296
            //child excute no inline: 1794
        }

        public void InterfaceExecTest()
        {
            Console.WriteLine("InterfaceExecTest");
            Stopwatch stopwatch = new();

            IMyInterface iMyClass = new MyClass();
            iMyClass.MyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                iMyClass.MyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"interface child excute: {stopwatch.ElapsedMilliseconds}");

            MyClass myClass = new();
            IMyInterface convertMyClass = myClass;
            convertMyClass.MyFunc();
            stopwatch.Restart();
            for (int i = 0; i < 1_000_000_000; i++)
            {
                convertMyClass.MyFunc();
            }
            stopwatch.Stop();
            Console.WriteLine($"convert child excute: {stopwatch.ElapsedMilliseconds}");

            //没有差别
            //InterfaceExecTest
            //interface child excute: 288
            //convert child excute: 291
        }
    }
}