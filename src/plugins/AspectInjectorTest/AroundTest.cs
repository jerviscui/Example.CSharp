using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    [Injection(typeof(UniversalWrapper))]
    public class UniversalWrapper : Attribute
    {
        private static readonly MethodInfo _asyncHandler = typeof(UniversalWrapper).GetMethod(nameof(UniversalWrapper.WrapAsync), BindingFlags.NonPublic | BindingFlags.Static)!;
        private static readonly MethodInfo _syncHandler = typeof(UniversalWrapper).GetMethod(nameof(UniversalWrapper.WrapSync), BindingFlags.NonPublic | BindingFlags.Static)!;
        private static readonly Type _voidTaskResult = Type.GetType("System.Threading.Tasks.VoidTaskResult")!;


        [Advice(Kind.Around, Targets = Target.Method)]
        public object? Handle(
            [Argument(Source.Metadata)] MethodBase methodInfo,
            [Argument(Source.Target)] Func<object[], object> target,
            [Argument(Source.Arguments)] object[] args,
            [Argument(Source.Name)] string name,
            [Argument(Source.ReturnType)] Type retType
            )
        {
            Console.WriteLine($"Before {name} {Thread.CurrentThread.ManagedThreadId.ToString()}");

            if (typeof(Task).IsAssignableFrom(retType) && methodInfo.GetCustomAttributes<AsyncStateMachineAttribute>().Any()) //check if method is async, you can also check by statemachine attribute
            {
                var syncResultType = retType.IsConstructedGenericType ? retType.GenericTypeArguments[0] : _voidTaskResult;
                var tgt = target;
                return _asyncHandler.MakeGenericMethod(syncResultType).Invoke(this, new object[] { tgt, args, name });
            }
            else
            {
                retType = retType == typeof(void) ? typeof(object) : retType;
                return _syncHandler.MakeGenericMethod(retType).Invoke(this, new object[] { target, args, name });
            }
        }

        private static T? WrapSync<T>(Func<object[], object> target, object[] args, string name)
        {
            try
            {
                var result = (T)target(args);
                Console.WriteLine($"After Sync method `{name}` {Thread.CurrentThread.ManagedThreadId.ToString()}");
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
                Console.WriteLine($"After Async method `{name}` {Thread.CurrentThread.ManagedThreadId.ToString()}");
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
        protected static readonly Dictionary<string, Delegate> Handlers = new Dictionary<string, Delegate>();

        protected static readonly MethodInfo AsyncHandler = typeof(AsyncAroundAspect)
            .GetMethod(nameof(WrapAsync), BindingFlags.NonPublic | BindingFlags.Static)!;
        protected static readonly MethodInfo SyncHandler = typeof(AsyncAroundAspect)
            .GetMethod(nameof(WrapSync), BindingFlags.NonPublic | BindingFlags.Static)!;

        public bool IsAsync(MethodBase methodInfo) => methodInfo.GetCustomAttributes<AsyncStateMachineAttribute>().Any();

        private string GetKey(Type type, string name, object[] args) =>
            $"{type.FullName}+{name}+{string.Join(",", args.Select(o => o.GetType().Name))}";

        public Delegate GetHandler(Type type, string name, object[] args, Type returnType)
        {
            var key = GetKey(type, name, args);
            if (Handlers.TryGetValue(key, out Delegate value))
            {
                return value;
            }
            else
            {
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
                if (!Handlers.ContainsKey(key))
                {
                    Handlers.TryAdd(key, value);
                }

                return value;
            }
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
        [Advice(Kind.Around, Targets = Target.Method)]
        public object Handle([Argument(Source.Instance)] object instance, [Argument(Source.Type)] Type type,
            [Argument(Source.Metadata)] MethodBase methodInfo, [Argument(Source.Target)] Func<object[], object> func,
            [Argument(Source.Name)] string targetName, [Argument(Source.Arguments)] object[] arguments,
            [Argument(Source.ReturnType)] Type returnType,
            [Argument(Source.Triggers)] Attribute[] triggers)
        {
            try
            {
                Console.WriteLine($"Before {targetName} {Thread.CurrentThread.ManagedThreadId.ToString()}");

                var result = func(arguments);
                if (typeof(Task).IsAssignableFrom(returnType))
                {
                    //Console.WriteLine(returnType.FullName);
                    if (result is Task task)
                    {
                        task.GetAwaiter().GetResult();
                    }
                }

                Console.WriteLine($"After {targetName} {Thread.CurrentThread.ManagedThreadId.ToString()}");
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
    public class AroundAttribute : Attribute
    {
    }

    [Around]
    public class AroundSyncTest
    {        
        public void Method()
        {
            Console.WriteLine($"Method {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ExceptionMethod()
        {
            throw new ArgumentException();
        }
    }

    //[Around]
    [UniversalWrapper]

    public class AroundAsyncTest
    {
        public async void AsyncMethod()
        {
            Console.WriteLine($"AsyncMethod {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        public Task TaskMehtod()
        {
            Console.WriteLine($"TaskMethod {Thread.CurrentThread.ManagedThreadId.ToString()}");

            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });

            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");
            return task;
        }

        public async Task AwaitTask()
        {
            Console.WriteLine($"AwaitTask {Thread.CurrentThread.ManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        public async Task<int> ContinueTask()
        {
            Console.WriteLine($"ContinuTask {Thread.CurrentThread.ManagedThreadId.ToString()}");

            var result = await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
                return 10;
            }).ContinueWith(t =>
            {
                Console.WriteLine($"continue Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
                return t.Result;
            });

            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");
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
