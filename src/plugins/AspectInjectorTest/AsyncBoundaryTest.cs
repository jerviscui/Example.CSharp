using System;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.Global)]
    public class MethodAspect
    {
        [Advice(Kind.Before, Targets = Target.Method | Target.Public | Target.Instance)]
        public void OnBefore([Argument(Source.Triggers)] Attribute[] attributes, 
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)]string name)
        {
            Console.WriteLine($"OnBefore {Thread.CurrentThread.ManagedThreadId}");
        }

        [Advice(Kind.After, Targets = Target.Method | Target.Public | Target.Instance)]
        public void OnAfter([Argument(Source.Triggers)] Attribute[] attributes, 
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)]string name)
        {
            Console.WriteLine($"OnAfter {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    [Injection(typeof(MethodAspect), Propagation = PropagateTo.Methods)]
    public class AsyncMethodAttribute : Attribute
    {

    }

    [AsyncMethod]
    public class AsyncBoundaryTest
    {
        public Task Test()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
            return Task.Delay(1000).ContinueWith(task => Console.WriteLine("Test"));
        }

        public async Task AwaitTest()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000);
            Console.WriteLine("AwaitTest");
        }

        public async Task AwaitTest2()
        {
            await Task.Delay(10);
        }
    }
}