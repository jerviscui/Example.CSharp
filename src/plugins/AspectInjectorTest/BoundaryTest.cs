using System;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    public class BoundaryAspect
    {
        [Advice(Kind.Before, Targets = Target.Method)]
        public void OnBefore([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name)
        {
            Console.WriteLine($"OnBefore {Thread.CurrentThread.ManagedThreadId}");
        }

        [Advice(Kind.After, Targets = Target.Method)]
        public void OnAfter([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name)
        {
            Console.WriteLine($"OnAfter {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    [Injection(typeof(BoundaryAspect), Propagation = PropagateTo.Methods)]
    public class BoundaryAttribute : Attribute
    {

    }

    [Boundary]
    public class BoundarySyncTest
    {
        public void Method()
        {
            Console.WriteLine($"Method {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    [Boundary]
    public class BoundaryAsyncTest
    {
        public async void AsyncMethod()
        {
            Console.WriteLine($"AsyncMethod {Thread.CurrentThread.ManagedThreadId}");
        }

        public Task TaskMehtod()
        {
            Console.WriteLine($"TaskMethod {Thread.CurrentThread.ManagedThreadId}");

            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId}");
            });

            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId}");
            return task;
        }

        public async Task AwaitTask()
        {
            Console.WriteLine($"AwaitTask {Thread.CurrentThread.ManagedThreadId}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId}");
            });
            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId}");
        }

        public async Task ContinueTask()
        {
            Console.WriteLine($"ContinuTask {Thread.CurrentThread.ManagedThreadId}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Thread.CurrentThread.ManagedThreadId}");
            });
            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId}");

            //OnBefore 4
            //ContinuTask 4
            //Task 5
            //continue Task 5
            //continuation 5
            //OnAfter 5
        }
    }
}