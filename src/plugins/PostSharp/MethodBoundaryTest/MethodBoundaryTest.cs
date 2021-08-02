using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using PostSharp.Aspects;
using PostSharp.Extensibility;

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

    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>", Scope = "member")]
    [MyBoundaryAspect(AttributeTargetElements = MulticastTargets.Method,
        AttributeTargetMemberAttributes = MulticastAttributes.Public)]
    public class SyncMethodBoundaryTest
    {
        public int Test()
        {
            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId.ToString()}");
            return 1;
        }

#pragma warning disable IDE0051 // 删除未使用的私有成员
        private Task PrivateTask()
#pragma warning restore IDE0051 // 删除未使用的私有成员
        {
            return Task.CompletedTask;
        }
    }

    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>", Scope = "member")]
    [MyBoundaryAspect(AttributeTargetElements = MulticastTargets.Method)]
    public class AsyncMethodBoundaryTest
    {
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
        public async void AsyncTest()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
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

    [SuppressMessage("Performance", "CA1822:将成员标记为 static", Justification = "<挂起>", Scope = "member")]
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
