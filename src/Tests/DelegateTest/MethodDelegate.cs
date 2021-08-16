using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq.Expressions;
using Common;

namespace DelegateTest
{
    internal class MethodDelegate
    {
        public static void DelegatePerformanceTest()
        {
            var b = new B();
            var method = typeof(B).GetMethod(nameof(B.Test))!;
            var d3 = method.CreateDelegate<Action>(b);

            int count = 1_000_000;

            var watch = new Stopwatch();

            watch.Restart();
            for (int i = 0; i < count; i++)
            {
                method.Invoke(b, null);
            }
            Print.Microsecond(watch, "invoke  :");

            watch.Restart();
            for (int i = 0; i < count; i++)
            {
                d3();
            }
            Print.Microsecond(watch, "delegate:");
        }

        public static void StaticMethodTest()
        {
            var method = typeof(B).GetMethod(nameof(B.Test))!;

            var b = new B();

            method.Invoke(b, null);

            //var d1 = (Action)method.CreateDelegate(typeof(Action));
            var d2 = (Action)method.CreateDelegate(typeof(Action), b);
            var d3 = method.CreateDelegate<Action>(b);

            d2();
            d3();

            //CreateDelegate 不传入实例
            var d33 = method.CreateDelegate<Action<B>>();
            d33(b);

            var method2 = typeof(B).GetMethod(nameof(B.STest))!;
            method2.Invoke(null, null);

            var d4 = (Action)method2.CreateDelegate(typeof(Action));
            d4();
        }

        public static void ExpressionTest()
        {
            var method = typeof(B).GetMethod(nameof(B.Test))!;

            //o => o.Test()
            var para = Expression.Parameter(typeof(B), "o");
            var body = Expression.Call(para, method, Array.Empty<Expression>());
            var exp = Expression.Lambda<Action<B>>(body, para);

            var lambda = exp.Compile();

            lambda(new B());
        }

        public static void GenericMethod()
        {
            var method = typeof(B).GetMethod(nameof(B.GenericMethod))!;

            var genericed = method.MakeGenericMethod(typeof(int));

            var b = new B();

            genericed.Invoke(b, null);

            var d1 = genericed.CreateDelegate<Func<int>>(b);
            d1();

            var d2 = genericed.CreateDelegate(typeof(Func<int>), b);
            //性能最差
            d2.DynamicInvoke();

            var d3 = (Func<int>)genericed.CreateDelegate(typeof(Func<int>), b);
            d3();

            var watch = new Stopwatch();
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                d1();
            }
            watch.Stop();
            Print.Microsecond(watch);

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                d2.DynamicInvoke();
            }
            watch.Stop();
            Print.Microsecond(watch);
            watch.Restart();
        }

        [C]
        private class A
        {
        }

        [C]
        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        private class B : A
        {
            [C]
            public void Test()
            {
                //Console.WriteLine("Test");
            }

            public static void STest()
            {
                //Console.WriteLine("STest");
            }

            public T? GenericMethod<T>()
            {
                return default;
            }
        }

        [AttributeUsage(AttributeTargets.All)]
        private class CAttribute : Attribute
        {
        }
    }
}
