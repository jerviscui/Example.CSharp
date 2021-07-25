using System;
using System.Diagnostics;
using System.Reflection;
using AspectInjector.Broker;
using Common;

namespace AspectInjectorTest
{
    public class PerformanceTests
    {
        //todo: new delegate invoke (args)
        //todo: delegate invoke (instance, args)

        [Aspect(Scope.PerInstance)]
        public class GetMethodAspect
        {
            [Advice(Kind.Before, Targets = Target.Method)]
            public void Before([Argument(Source.Metadata)] MethodBase methodInfo)
            {

            }
        }

        [Injection(typeof(GetMethodAspect))]
        public class GetMethodAttribute : Attribute
        {

        }

        class Foo
        {
            [GetMethod]
            public int ReturnInt()
            {
                return 1;
            }

            public int ReturnIntDirectly()
            {
                return 1;
            }
        }

        public void GetMethod_ReflectionPerformance_Test()
        {
            var stopwatch = new Stopwatch();
            var foo = new Foo();

            stopwatch.Start();
            for (int i = 0; i < 1_000_000; i++)
            {
                foo.ReturnInt();
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch, "Reflection:");

            stopwatch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                foo.ReturnIntDirectly();
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch, "Directly  :");
        }
    }
}