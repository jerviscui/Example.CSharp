using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class BoundaryAspect
    {
        [Advice(Kind.Before, Targets = Target.Method)]
        public void OnBefore([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name)
        {
            Console.WriteLine($"OnBefore {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void OnAfter([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name)
        {
            Console.WriteLine($"OnAfter {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }
    }

    [Injection(typeof(BoundaryAspect), Propagation = PropagateTo.Methods)]
    [AttributeUsage(AttributeTargets.All)]
    public class BoundaryAttribute : Attribute
    {
    }

    [Boundary]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class BoundarySyncTest
    {
        public void Method()
        {
            Console.WriteLine($"Method {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }
    }

    [Boundary]
    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>")]
    public class BoundaryAsyncTest
    {
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public async void AsyncMethod()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
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

        public async Task ContinueTask()
        {
            Console.WriteLine($"ContinuTask {Thread.CurrentThread.ManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");

            //OnBefore 4
            //ContinuTask 4
            //Task 5
            //continue Task 5
            //continuation 5
            //OnAfter 5
        }
    }
}
