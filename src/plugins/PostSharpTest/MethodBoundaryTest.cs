using PostSharp.Aspects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace PostSharpTest
{
    [Serializable]
    public class MyBoundaryAspectAttribute : OnMethodBoundaryAspect
    {
        /// <inheritdoc />
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnEntry {Thread.CurrentThread.ManagedThreadId}");
        }

        /// <inheritdoc />
        public override void OnSuccess(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnSuccess {Thread.CurrentThread.ManagedThreadId}");
        }

        /// <inheritdoc />
        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnExit {Thread.CurrentThread.ManagedThreadId}");
        }

        /// <inheritdoc />
        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnException {Thread.CurrentThread.ManagedThreadId}");
        }

        /// <inheritdoc />
        public override void OnResume(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnResume {Thread.CurrentThread.ManagedThreadId}");
        }

        /// <inheritdoc />
        public override void OnYield(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnYield {Thread.CurrentThread.ManagedThreadId}");
        }
    }

    [MyBoundaryAspect]
    public class MethodBoundaryTest
    {
        public void Test()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
        }

        public Task TaskTest()
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

        public async Task<int> AwaitTest2()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}");
            await Task.Delay(1000);
            Console.WriteLine("AwaitTest");

            return 1;
        }
    }
}
