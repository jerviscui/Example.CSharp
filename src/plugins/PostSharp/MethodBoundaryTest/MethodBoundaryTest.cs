using PostSharp.Aspects;
using PostSharp.Extensibility;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MethodBoundaryTest
{
    [Serializable]
    public class MyBoundaryAspectAttribute : OnMethodBoundaryAspect
    {
        /// <inheritdoc />
        public override void OnEntry(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnEntry {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnSuccess(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnSuccess {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnExit(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnExit {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnException(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnException {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnResume(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnResume {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnYield(MethodExecutionArgs args)
        {
            Console.WriteLine($"OnYield {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }
    }

    [MyBoundaryAspect(AttributeTargetElements = MulticastTargets.Method,
        AttributeTargetMemberAttributes = MulticastAttributes.Public)]
    public class SyncMethodBoundaryTest
    {
        public int Test()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString()}");
            return 1;
        }

        private Task PrivateTask()
        {
            return Task.CompletedTask;
        }
    }

    [MyBoundaryAspect(AttributeTargetElements = MulticastTargets.Method)]
    public class AsyncMethodBoundaryTest
    {
        public async void AsyncTest()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        public Task NoneAwaitTaskTest()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString()}");
            return Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"NoneAwaitTaskTest {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });
        }

        public async Task AwaitTaskTest()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Console.WriteLine($"AwaitTaskTest {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });
        }
    }

    [MyBoundaryAspect(AttributeTargetElements = MulticastTargets.Method)]
    public class TaskMethodBoundaryTest
    {
        public async Task<int> ContinuationTest()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"continuation task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });

            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");

            return 1;
        }
    }
}
