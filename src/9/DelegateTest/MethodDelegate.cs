using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DelegateTest
{
    class MethodDelegate
    {
        public static void DelegatePerformanceTest()
        {
            var b = new B();
            var method = typeof(B).GetMethod(nameof(B.Test))!;
            var d3 = method.CreateDelegate<Action>(b);

            int count = 1000_000;

            var watch = new Stopwatch();

            watch.Restart();
            for (int i = 0; i < count; i++)
            {
                method.Invoke(b, null);
            }
            Console.WriteLine($"invoke  : {watch.ElapsedTicks / 10,10:##,###} us");

            watch.Restart();
            for (int i = 0; i < count; i++)
            {
                d3();
            }
            Console.WriteLine($"delegate: {watch.ElapsedTicks / 10,10:##,###} us");
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

        [C]
        class A
        {

        }

        [C]
        class B : A
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
        }

        class CAttribute : Attribute
        {

        }
    }
}
