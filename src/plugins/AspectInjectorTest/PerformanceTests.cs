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

        #region ReflectionPerformance

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

        #endregion

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

            //反射获取 Source.Metadata 性能降低 400 倍
            //Reflection:    122,044 us
            //Directly  :        279 us
        }

        #region DeclareType vs typeof

        [Aspect(Scope.PerInstance)]
        public class DeclareTypeAspect
        {
            [Advice(Kind.Around, Targets = Target.Method)]
            public object Around([Argument(Source.Target)] Func<object[], object> target, [Argument(Source.Arguments)] object[] arguments)
            {
                var name = target.Method.DeclaringType?.FullName;

                return target.Invoke(arguments);
            }
        }

        [Injection(typeof(GetMethodAspect))]
        public class DeclareTypeAttribute : Attribute
        {

        }

        [Aspect(Scope.PerInstance)]
        public class TypeofAspect
        {
            [Advice(Kind.Around, Targets = Target.Method)]
            public object Around([Argument(Source.Target)] Func<object[], object> target, [Argument(Source.Arguments)] object[] arguments, 
                [Argument(Source.Type)] Type type)
            {
                var name = type.FullName;

                return target.Invoke(arguments);
            }
        }

        [Injection(typeof(TypeofAspect))]
        public class TypeofAttribute : Attribute
        {

        }

        class TypeofTestClass
        {
            [DeclareType]
            public int DeclareTypeMehtod(int i)
            {
                return i;
            }

            [Typeof]
            public int TypeofMehtod(int i)
            {
                return i;
            }
        }

        #endregion

        public void TestClass_GetDeclareType_Test()
        {
            var stopwatch = new Stopwatch();
            var t = new TypeofTestClass();

            stopwatch.Start();
            for (int i = 0; i < 1_000_000; i++)
            {
                t.DeclareTypeMehtod(1);
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch, "DeclareType:");

            stopwatch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                t.TypeofMehtod(2);
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch, "Typeof     :");

            //MethodInfo.Method.DeclaringType 会使用反射，比 typeof 性能低 50%
            //DeclareType:    116,011 us
            //Typeof     :     75,680 us
        }
    }
}