using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AspectInjector.Broker;

namespace AspectInjectorTest
{
    [Aspect(Scope.PerInstance)]
    [Injection(typeof(AroundAttribute))]
    public class AroundAttribute : Attribute
    {
        [Advice(Kind.Around, Targets = Target.Method)]
        public object Handle([Argument(Source.Triggers)] Attribute[] attributes,
            [Argument(Source.Arguments)] object[] arguments, [Argument(Source.Name)] string name,
            [Argument(Source.Instance)] object instance, [Argument(Source.Target)] Func<object[], object> method)
        {
            try
            {
                Console.WriteLine($"Before {name} {Thread.CurrentThread.ManagedThreadId}");
                var result = method(arguments);
                Console.WriteLine($"After {name} {Thread.CurrentThread.ManagedThreadId}");
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    [Around]
    public class AsyncAroundTest
    {
        public void SyncTest()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
            throw new ArgumentException();
        }

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

        public Task ExceptionTest()
        {
            throw new ArgumentException();
            return Task.Delay(10);
        }

        public async Task ExceptionAwaitTest(bool isThrow)
        {
            if (isThrow)
            {
                throw new ArgumentException();
            }

            await Task.Delay(10);
        }
    }
}
