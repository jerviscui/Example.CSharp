using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
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

        [Injection(typeof(DeclareTypeAspect))]
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

            //MethodInfo.Method.DeclaringType 会使用反射，比 typeof 性能低 4.5 倍
            //DeclareType:    283,310 us
            //Typeof     :     62,498 us
        }

        #region Delegate Cache

        [Aspect(Scope.PerInstance)]
        public class DelegateNewAspect
        {
            [Advice(Kind.Around, Targets = Target.Method)]
            public object Around([Argument(Source.Target)] Func<object[], object> target, [Argument(Source.Arguments)] object[] arguments)
            {
                return target.Invoke(arguments);
            }
        }

        [Injection(typeof(DelegateNewAspect))]
        public class DelegateNewAttribute : Attribute
        {

        }

        class DelegateCacheTestClass
        {
            [DelegateNew]
            public int Method(int i)
            {
                return i;
            }

            public int Method2(int i)
            {
                return i;
            }

            private object MethodWrap(object[] array)
            {
                return Method2((int)array[0]);
            }

            public int MethodWithInstance(int i)
            {
                Func<DelegateCacheTestClass, object[], object> func = (o, args) => o.MethodWrap(args);

                return (int)func.Invoke(this, new object[] { i });
            }

            private static Func<DelegateCacheTestClass, object[], object>? funcCache;

            public int MethodByCache(int i)
            {
                //和 MethodWithInstance() 方法没有区别
                if (funcCache is null)
                {
                    var method = typeof(DelegateCacheTestClass).GetMethod(nameof(MethodWrap), BindingFlags.Instance | BindingFlags.NonPublic)!;
                    funcCache = (Func<DelegateCacheTestClass, object[], object>)
                        Delegate.CreateDelegate(typeof(Func<DelegateCacheTestClass, object[], object>), method);
                }

                return (int)funcCache.Invoke(this, new object[] { i });
            }
        }

        #endregion

        public void DelegateCacheTestClass_MethodWithInstance_Test()
        {
            var stopwatch = new Stopwatch();
            var t = new DelegateCacheTestClass();

            stopwatch.Start();
            for (int i = 0; i < 1_000_000; i++)
            {
                //t.Method(1);
                t.MethodWithInstance(2);
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch, "DelegateCache:");

            stopwatch.Restart();
            for (int i = 0; i < 1_000_000; i++)
            {
                t.Method(1);
                //t.MethodWithInstance(2);
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch, "Delegate     :");

            //将 Func<> 缓存之后，性能提升 30%
            //DelegateCache:     24,905 us
            //Delegate     :     31,339 us
        }
    }
}