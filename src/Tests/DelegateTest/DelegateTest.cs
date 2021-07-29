using Common;
using System;
using System.Diagnostics;

namespace DelegateTest
{
    public class DelegateTest
    {
        public void GetMehtodTest()
        {
            var watch = new Stopwatch();
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var a = new A();
                //(MethodBase)MethodBase.GetMethodFromHandle(methodof(A.Test()).MethodHandle, typeof(A).TypeHandle)
                var method = typeof(A).GetMethod("Test");
            }
            watch.Stop();
            Print.Microsecond(watch, "GetMethod");

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var a = new A();
                var method = a.GetAction();
            }
            watch.Stop();
            Print.Microsecond(watch, "new Delegate");

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var a = new A();
                var method = a.GetActionCache();
            }
            watch.Stop();
            Print.Microsecond(watch, "cache Delegate");

            //GetMethod         84,269 us
            //new Delegate      21,072 us
            //cache Delegate    11,550 us
        }

        public void GetAndExecTest()
        {
            var watch = new Stopwatch();
            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var a = new A();
                //(MethodBase)MethodBase.GetMethodFromHandle(methodof(A.Test()).MethodHandle, typeof(A).TypeHandle)
                var method = typeof(A).GetMethod("Test")!;
                method.Invoke(a, Array.Empty<object>());
            }
            watch.Stop();
            Print.Microsecond(watch, "GetMethod");

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var a = new A();
                var method = a.GetAction();
                method();
            }
            watch.Stop();
            Print.Microsecond(watch, "new Delegate");

            watch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                var a = new A();
                var method = a.GetActionCache();
                method();
            }
            watch.Stop();
            Print.Microsecond(watch, "cache Delegate");
        }

        public class A
        {
            public void Test()
            {

            }

            public Action GetAction()
            {
                return new Action(Test);
            }

            private static Action? _action;

            public Action GetActionCache()
            {
                return _action ??= new Action(Test);
            }
        }
    }
}
