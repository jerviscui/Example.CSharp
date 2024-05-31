using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using MethodBoundaryAspect.Fody.Attributes;

namespace MethodBoundaryAspectTest
{
    public class TaskLogAttribute : OnMethodBoundaryAspect
    {
        /// <inheritdoc />
        public override void OnEntry(MethodExecutionArgs arg)
        {
            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} OnEntry {Environment.CurrentManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnExit(MethodExecutionArgs arg)
        {
            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} OnExit {Environment.CurrentManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnException(MethodExecutionArgs arg)
        {
            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} OnException {Environment.CurrentManagedThreadId.ToString()}");
            arg.FlowBehavior = FlowBehavior.Continue;
        }
    }

    [TaskLog]
    public class AsyncTest
    {
        //todo: not implement
        //public async void SyncMethod()
        //{
        //    Console.WriteLine($"front {Environment.CurrentManagedThreadId.ToString()}");
        //}

#pragma warning disable CA1822 // Mark members as static
        public Task TaskMethod()
#pragma warning restore CA1822 // Mark members as static
        {
            Console.WriteLine($"TaskMethod {Environment.CurrentManagedThreadId.ToString()}");

            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
            });

            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");
            return task;

            //OnEntry 1
            //TaskMethod 1
            //continuation 1
            //OnExit 1
            //Task 4
        }

#pragma warning disable CA1822 // Mark members as static
        public async Task AwaitTask()
#pragma warning restore CA1822 // Mark members as static
        {
            Console.WriteLine($"AwaitTask {Environment.CurrentManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");
        }

#pragma warning disable CA1822 // Mark members as static
        public async Task ContinueTask()
#pragma warning restore CA1822 // Mark members as static
        {
            Console.WriteLine($"ContinuTask {Environment.CurrentManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Environment.CurrentManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");

            //AsyncTest.ContinueTask OnEntry 1
            //<ContinueTask>d__2.MoveNext OnEntry 1
            //ContinuTask 1
            //<ContinueTask>d__2.MoveNext OnExit 1
            //AsyncTest.ContinueTask OnExit 1
            //Task 4
            //continue Task 5
            //<ContinueTask>d__2.MoveNext OnEntry 5
            //continuation 5
            //<ContinueTask>d__2.MoveNext OnExit 5
        }
    }

    public class AsyncTest2
    {
        [TaskLog]
#pragma warning disable CA1822 // Mark members as static
        public async Task<int> ContinueTaskAndReturn()
#pragma warning restore CA1822 // Mark members as static
        {
            Console.WriteLine($"ContinueTaskAndReturn {Environment.CurrentManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Environment.CurrentManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");

            return 1;

            //AsyncTest2.ContinueTaskAndReturn OnEntry 1
            //ContinueTaskAndReturn 1
            //AsyncTest2.ContinueTaskAndReturn OnExit 1
            //Task 4
            //continue Task 5
            //continuation 5
        }
    }

    [TaskLog(AttributeTargetMemberAttributes = MulticastAttributes.Public)]
    public class AsyncTest3
    {
#pragma warning disable CA1822 // Mark members as static
        public async Task<int> ContinueTaskAndReturn()
#pragma warning restore CA1822 // Mark members as static
        {
            Console.WriteLine($"ContinueTaskAndReturn {Environment.CurrentManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Environment.CurrentManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");

            return 1;

            //AsyncTest3.ContinueTaskAndReturn OnEntry 5
            //ContinueTaskAndReturn 5
            //AsyncTest3.ContinueTaskAndReturn OnExit 5
            //Task 6
            //continue Task 6
            //continuation 6
        }
    }

    public class TaskLog2Attribute : OnMethodBoundaryAspect
    {
        /// <inheritdoc />
        public override void OnEntry(MethodExecutionArgs arg)
        {
            int state = -100;
            if (arg.Instance is IAsyncStateMachine)
            {
                state = (int)(arg.Instance.GetType().GetField("<>1__state")?.GetValue(arg.Instance) ?? -100);
            }

            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} {state} OnEntry {Environment.CurrentManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnExit(MethodExecutionArgs arg)
        {
            int state = -100;
            if (arg.Instance is IAsyncStateMachine)
            {
                state = (int)(arg.Instance.GetType().GetField("<>1__state")?.GetValue(arg.Instance) ?? -100);
            }

            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} {state} OnExit {Environment.CurrentManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnException(MethodExecutionArgs arg)
        {
            int state = -100;
            if (arg.Instance is IAsyncStateMachine)
            {
                state = (int)(arg.Instance.GetType().GetField("<>1__state")?.GetValue(arg.Instance) ?? -100);
            }

            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} {state} OnException {Environment.CurrentManagedThreadId.ToString()}");
            arg.FlowBehavior = FlowBehavior.Continue;
        }
    }

    [TaskLog2]
    public class AsyncTest4
    {
#pragma warning disable CA1822 // Mark members as static
        public async Task<int> ContinueTaskAndReturn()
#pragma warning restore CA1822 // Mark members as static
        {
            Console.WriteLine($"ContinueTaskAndReturn {Environment.CurrentManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Environment.CurrentManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Environment.CurrentManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Environment.CurrentManagedThreadId.ToString()}");

            return 1;

            //AsyncTest4.ContinueTaskAndReturn -100 OnEntry 6
            //<ContinueTaskAndReturn>d__0.MoveNext -1 OnEntry 6
            //ContinueTaskAndReturn 6
            //<ContinueTaskAndReturn>d__0.MoveNext 0 OnExit 6
            //AsyncTest4.ContinueTaskAndReturn -100 OnExit 6
            //Task 5
            //continue Task 5
            //<ContinueTaskAndReturn>d__0.MoveNext 0 OnEntry 5
            //continuation 5
            //<ContinueTaskAndReturn>d__0.MoveNext -2 OnExit 5
        }
    }
}
