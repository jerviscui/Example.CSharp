using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using AspectInjector.Broker;
using Common;

namespace AspectInjectorTest
{
    public class PerformanceTests
    {
        #region ReflectionPerformance

        [Aspect(Scope.PerInstance)]
        public class GetMethodAspect
        {
            [Advice(Kind.Before, Targets = Target.Method)]
            [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
            public void Before([Argument(Source.Metadata)] MethodBase methodInfo)
            {
            }
        }

        [Injection(typeof(GetMethodAspect))]
        [AttributeUsage(AttributeTargets.All)]
        public class GetMethodAttribute : Attribute
        {
        }

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        private class Foo
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

        public static void GetMethod_ReflectionPerformance_Test()
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
            [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
            public object Around([Argument(Source.Target)] Func<object[], object> target,
                [Argument(Source.Arguments)] object[] arguments)
            {
                var name = target.Method.DeclaringType?.FullName;

                return target.Invoke(arguments);
            }
        }

        [Injection(typeof(DeclareTypeAspect))]
        [AttributeUsage(AttributeTargets.All)]
        public class DeclareTypeAttribute : Attribute
        {
        }

        [Aspect(Scope.PerInstance)]
        public class TypeofAspect
        {
            [Advice(Kind.Around, Targets = Target.Method)]
            [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
            public object Around([Argument(Source.Target)] Func<object[], object> target,
                [Argument(Source.Arguments)] object[] arguments,
                [Argument(Source.Type)] Type type)
            {
                var name = type.FullName;

                return target.Invoke(arguments);
            }
        }

        [Injection(typeof(TypeofAspect))]
        [AttributeUsage(AttributeTargets.All)]
        public class TypeofAttribute : Attribute
        {
        }

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        private class TypeofTestClass
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

        public static void TestClass_GetDeclareType_Test()
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
            [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
            public object Around([Argument(Source.Target)] Func<object[], object> target,
                [Argument(Source.Arguments)] object[] arguments)
            {
                return target.Invoke(arguments);
            }
        }

        [Injection(typeof(DelegateNewAspect))]
        [AttributeUsage(AttributeTargets.All)]
        public class DelegateNewAttribute : Attribute
        {
        }

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        private class DelegateCacheTestClass
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
#pragma warning disable IDE0039 // 使用本地函数
                Func<DelegateCacheTestClass, object[], object> func = (o, args) => o.MethodWrap(args);
#pragma warning restore IDE0039 // 使用本地函数
                return (int)func.Invoke(this, new object[] { i });

                //本地函数要比 Delegate 性能高 10%
                //object Func(DelegateCacheTestClass o, object[] args) => o.MethodWrap(args);
                //return (int)Func(this, new object[] { i });
            }

            private static Func<DelegateCacheTestClass, object[], object>? _funcCache;

            public int MethodByCache(int i)
            {
                //和 MethodWithInstance() 方法没有区别，MethodWithInstance() 默认使用了静态缓存
                if (_funcCache is null)
                {
                    var method = typeof(DelegateCacheTestClass).GetMethod(nameof(MethodWrap),
                        BindingFlags.Instance | BindingFlags.NonPublic)!;
                    _funcCache = (Func<DelegateCacheTestClass, object[], object>)
                        Delegate.CreateDelegate(typeof(Func<DelegateCacheTestClass, object[], object>), method);
                }

                return (int)_funcCache.Invoke(this, new object[] { i });
            }
        }

        #endregion

        public static void DelegateCacheTestClass_MethodWithInstance_Test()
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
