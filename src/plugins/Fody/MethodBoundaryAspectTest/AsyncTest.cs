using MethodBoundaryAspect.Fody.Attributes;
using System;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace MethodBoundaryAspectTest
{
    public class TaskLogAttribute : OnMethodBoundaryAspect
    {
        /// <inheritdoc />
        public override void OnEntry(MethodExecutionArgs arg)
        {
            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} OnEntry {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnExit(MethodExecutionArgs arg)
        {
            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} OnExit {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnException(MethodExecutionArgs arg)
        {
            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} OnException {Thread.CurrentThread.ManagedThreadId.ToString()}");
            arg.FlowBehavior = FlowBehavior.Continue;
        }
    }

    [TaskLog]
    public class AsyncTest
    {
        //todo: not implement
        //public async void SyncMethod()
        //{
        //    Console.WriteLine($"front {Thread.CurrentThread.ManagedThreadId.ToString()}");
        //}

        public Task TaskMethod()
        {
            Console.WriteLine($"TaskMethod {Thread.CurrentThread.ManagedThreadId.ToString()}");

            var task = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });

            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");
            return task;

            //OnEntry 1
            //TaskMethod 1
            //continuation 1
            //OnExit 1
            //Task 4
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
        public async Task<int> ContinueTaskAndReturn()
        {
            Console.WriteLine($"ContinueTaskAndReturn {Thread.CurrentThread.ManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");

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
        public async Task<int> ContinueTaskAndReturn()
        {
            Console.WriteLine($"ContinueTaskAndReturn {Thread.CurrentThread.ManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");

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

            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} {state} OnEntry {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnExit(MethodExecutionArgs arg)
        {
            int state = -100;
            if (arg.Instance is IAsyncStateMachine)
            {
                state = (int)(arg.Instance.GetType().GetField("<>1__state")?.GetValue(arg.Instance) ?? -100);
            }

            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} {state} OnExit {Thread.CurrentThread.ManagedThreadId.ToString()}");
        }

        /// <inheritdoc />
        public override void OnException(MethodExecutionArgs arg)
        {
            int state = -100;
            if (arg.Instance is IAsyncStateMachine)
            {
                state = (int)(arg.Instance.GetType().GetField("<>1__state")?.GetValue(arg.Instance) ?? -100);
            }

            Console.WriteLine($"{arg.Instance.GetType().Name}.{arg.Method.Name} {state} OnException {Thread.CurrentThread.ManagedThreadId.ToString()}");
            arg.FlowBehavior = FlowBehavior.Continue;
        }
    }

    [TaskLog2]
    public class AsyncTest4
    {
        public async Task<int> ContinueTaskAndReturn()
        {
            Console.WriteLine($"ContinueTaskAndReturn {Thread.CurrentThread.ManagedThreadId.ToString()}");
            await Task.Factory.StartNew(() =>
            {
                Thread.Sleep(100);
                Console.WriteLine($"Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            }).ContinueWith(_ =>
            {
                Console.WriteLine($"continue Task {Thread.CurrentThread.ManagedThreadId.ToString()}");
            });
            Console.WriteLine($"continuation {Thread.CurrentThread.ManagedThreadId.ToString()}");

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
