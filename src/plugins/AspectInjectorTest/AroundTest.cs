using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    [Injection(typeof(UniversalWrapper))]
    [AttributeUsage(AttributeTargets.All)]
    public class UniversalWrapper : Attribute
    {
        private static readonly MethodInfo AsyncHandler =
            typeof(UniversalWrapper).GetMethod(nameof(WrapAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

        private static readonly MethodInfo SyncHandler =
            typeof(UniversalWrapper).GetMethod(nameof(WrapSync), BindingFlags.NonPublic | BindingFlags.Static)!;

        private static readonly Type VoidTaskResult = Type.GetType("System.Threading.Tasks.VoidTaskResult")!;

        [Advice(Kind.Around, Targets = Target.Method)]
        public object? Handle([Argument(Source.Metadata)] MethodBase methodInfo,
            [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Arguments)] object[] args,
            [Argument(Source.Name)] string name,
            [Argument(Source.ReturnType)] Type retType)
        {
            Console.WriteLine($"Before {name} {Environment.CurrentManagedThreadId.ToString()}");

            if (typeof(Task).IsAssignableFrom(retType) &&
                methodInfo.GetCustomAttributes<AsyncStateMachineAttribute>().Any()
            ) //check if method is async, you can also check by statemachine attribute
            {
                var syncResultType =
                    retType.IsConstructedGenericType ? retType.GenericTypeArguments[0] : VoidTaskResult;
                var tgt = target;
                return AsyncHandler.MakeGenericMethod(syncResultType).Invoke(this, new object[] { tgt, args, name });
            }
            retType = retType == typeof(void) ? typeof(object) : retType;
            return SyncHandler.MakeGenericMethod(retType).Invoke(this, new object[] { target, args, name });
        }

        private static T? WrapSync<T>(Func<object[], object> target, object[] args, string name)
        {
            try
            {
                var result = (T)target(args);
                Console.WriteLine($"After Sync method `{name}` {Environment.CurrentManagedThreadId.ToString()}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Sync method `{name}` throws {e.GetType()} exception.");
                return default;
            }
        }

        private static async Task<T?> WrapAsync<T>(Func<object[], object> target, object[] args, string name)
        {
            try
            {
                var result = await ((Task<T>)target(args)).ConfigureAwait(false);
                Console.WriteLine($"After Async method `{name}` {Environment.CurrentManagedThreadId.ToString()}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Async method `{name}` throws {e.GetType()} exception.");
                return default;
            }
        }
    }

    //todo: 实现异步方法 around，参考：https://github.com/pamidur/aspect-injector/blob/master/samples/UniversalWrapper/UniversalWrapper.cs
    public class AsyncAroundAspect
    {
        private static readonly Dictionary<string, Delegate> Handlers = new();

        private static readonly MethodInfo AsyncHandler = typeof(AsyncAroundAspect)
            .GetMethod(nameof(WrapAsync), BindingFlags.NonPublic | BindingFlags.Static)!;

        protected static readonly MethodInfo SyncHandler = typeof(AsyncAroundAspect)
            .GetMethod(nameof(WrapSync), BindingFlags.NonPublic | BindingFlags.Static)!;

        public static bool IsAsync(MethodBase methodInfo) =>
            methodInfo.GetCustomAttributes<AsyncStateMachineAttribute>().Any();

        private static string GetKey(Type type, string name, object[] args) =>
            $"{type.FullName}+{name}+{string.Join(",", args.Select(o => o.GetType().Name))}";

        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        public Delegate GetHandler(Type type, string name, object[] args, Type returnType)
        {
            var key = GetKey(type, name, args);
            if (Handlers.TryGetValue(key, out var value))
            {
                return value;
            }
            //make generic method
            var method = AsyncHandler.MakeGenericMethod(returnType);

            //create delegate
            //<Func<Func<object[], object>, object[], string, Task<T>>>
            //delegateType = typeof(Func<>).MakeGenericType(Func<object[], object>, object[], string, Task<T>)
            //get CreateDelegate method
            //value = method.CreateDelegate(type);
            //make generic CreateDelegate method
            //invoke CreateDelegate method

            //add
            value = null!;
            if (!Handlers.ContainsKey(key))
            {
                Handlers.TryAdd(key, value);
            }

            return value;
        }

        private static T WrapSync<T>(Func<object[], object> target, object[] args, string name)
        {
            var result = (T)target(args);
            Console.WriteLine($"Sync method `{name}` completes successfuly.");
            return result;
        }

        private static async Task<T> WrapAsync<T>(Func<object[], object> target, object[] args, string name)
        {
            var result = await ((Task<T>)target(args)).ConfigureAwait(false);
            Console.WriteLine($"Async method `{name}` completes successfuly.");
            return result;
        }
    }

    [Aspect(Scope.Global)]
    public class AroundAspect
    {
        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        [Advice(Kind.Around, Targets = Target.Method)]
        public object Handle([Argument(Source.Instance)] object instance, [Argument(Source.Type)] Type type,
            [Argument(Source.Metadata)] MethodBase methodInfo, [Argument(Source.Target)] Func<object[], object> func,
            [Argument(Source.Name)] string targetName, [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.ReturnType)] Type returnType,
            [Argument(Source.Triggers)] Attribute[] triggers)
        {
            try
            {
                Console.WriteLine($"Before {targetName} {Environment.CurrentManagedThreadId.ToString()}");

                var result = func(arguments);
                if (typeof(Task).IsAssignableFrom(returnType))
                {
                    //Console.WriteLine(returnType.FullName);
                    if (result is Task task)
                    {
                        task.GetAwaiter().GetResult();
                    }
                }

                Console.WriteLine($"After {targetName} {Environment.CurrentManagedThreadId.ToString()}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        //todo: 1. how about a separate async method
        //2. use out parameter for return result 
        [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
        public async Task Handle(
        //[Argument(Source.Instance)] object instance, [Argument(Source.Type)] Type type,
        //[Argument(Source.Metadata)] MethodBase methodInfo, [Argument(Source.Target)] Func<object[], object> func,
        //[Argument(Source.Name)] string targetName, [Argument(Source.Arguments)] object[] arguments,
        //[Argument(Source.ReturnType)] Type returnType,
        //[Argument(Source.Triggers)] Attribute[] triggers
        )
        {
            try
            {
                await Task.CompletedTask;
                //set result
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    [Injection(typeof(AroundAspect))]
    [AttributeUsage(AttributeTargets.All)]
    public class AroundAttribute : Attribute
    {
    }

    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    [Around]
    public class AroundSyncTest
    {
        public void Method()
        {
            Console.WriteLine($"Method {Environment.CurrentManagedThreadId.ToString()}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ExceptionMethod()
        {
            throw new ArgumentException();
        }
    }

    //[Around]
    [UniversalWrapper]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class AroundAsyncTest
    {
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public async void AsyncMethod()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        {
            Console.WriteLine($"AsyncMethod {Environment.CurrentManagedThreadId.ToString()}");
        }

        public Task TaskMehtod()
        {
            Console.WriteLine($"TaskMethod {Environment.CurrentManagedThreadId.ToString()}");

            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
            });

            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");
            return task;
        }

        public async Task AwaitTask()
        {
            Console.WriteLine($"AwaitTask {Environment.CurrentManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");
        }

        public async Task<int> ContinueTask()
        {
            Console.WriteLine($"ContinuTask {Environment.CurrentManagedThreadId.ToString()}");

            var result = await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
                return 10;
            }).ContinueWith(t =>
            {
                Console.WriteLine($"continue Task {Environment.CurrentManagedThreadId.ToString()}");
                return t.Result;
            });

            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");
            return result;

            //Before ContinueTask 1
            //ContinuTask 1
            //Task 4
            //continue Task 5
            //continuation 5
            //After ContinueTask 1
            //result 10
        }
    }
}
