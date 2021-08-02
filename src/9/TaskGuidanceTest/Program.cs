using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    internal partial class Program
    {
        private static void Main(string[] args)
        {
            //var t = TaskTest.SimpleTask2();
            //var awaiter = t.GetAwaiter();
            //awaiter.GetResult(); //阻塞主线程

            //SynchronizationContextTest.Test();

            //CultureTest.Test();

            ChildTaskTest.Test();
        }

        public class TaskTest
        {
#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
            public static async void Void()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
            {
                Console.WriteLine("");
            }

#pragma warning disable CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
            public static async Task SimpleTask()
#pragma warning restore CS1998 // 异步方法缺少 "await" 运算符，将以同步方式运行
            {
                Console.WriteLine("SimpleTask");
            }

            public static async Task SimpleTask2()
            {
                //1
                Console.WriteLine($"SimpleTask2 {Thread.CurrentThread.ManagedThreadId.ToString()}");
                await Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000 * 10);
                    //5
                    Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId.ToString()}");
                }, TaskCreationOptions.LongRunning);
                var s = new StackTrace().GetFrames();
                foreach (var stackFrame in s)
                {
                    var m = stackFrame.GetMethod()!;
                    Console.WriteLine($"{m.Module,-28} {m.DeclaringType,-103} {m.Name}");
                }
                //5
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId.ToString()}");
                //调用堆栈输出
                //TaskGuidanceTest.dll         TaskGuidanceTest.Program+TaskTest+<SimpleTask2>d__2                                                     MoveNext
                //System.Private.CoreLib.dll   System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1+AsyncStateMachineBox`1[TResult,TStateMachine]  ExecutionContextCallback
                //System.Private.CoreLib.dll   System.Threading.ExecutionContext                                                                       RunInternal
                //System.Private.CoreLib.dll   System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1+AsyncStateMachineBox`1[TResult,TStateMachine]  MoveNext
                //System.Private.CoreLib.dll   System.Runtime.CompilerServices.AsyncTaskMethodBuilder`1+AsyncStateMachineBox`1[TResult,TStateMachine]  MoveNext
                //System.Private.CoreLib.dll   System.Runtime.CompilerServices.TaskAwaiter+<>c                                                         <OutputWaitEtwEvents>b__12_0
                //System.Private.CoreLib.dll   System.Runtime.CompilerServices.AsyncMethodBuilderCore+ContinuationWrapper                              Invoke
                //System.Private.CoreLib.dll   System.Threading.Tasks.AwaitTaskContinuation                                                            RunOrScheduleAction
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                                                                             RunContinuations
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                                                                             FinishContinuations
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                                                                             FinishStageThree
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                                                                             FinishStageTwo
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                                                                             ExecuteWithThreadLocal
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                                                                             ExecuteEntryUnsafe
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                                                                             ExecuteFromThreadPool
                //System.Private.CoreLib.dll   System.Threading.ThreadPoolWorkQueue                                                                    Dispatch
                //System.Private.CoreLib.dll   System.Threading._ThreadPoolWaitCallback                                                                PerformWaitCallback
            }

            public static async Task ContinuationTask()
            {
                //1
                Console.WriteLine($"ChildTask {Thread.CurrentThread.ManagedThreadId.ToString()}");
                await Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000 * 1);
                    //4
                    Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId.ToString()}");
                }).ContinueWith(task =>
                {
                    //child task
                    //5
                    Console.WriteLine($"child {Thread.CurrentThread.ManagedThreadId.ToString()}");

                    var s = new StackTrace().GetFrames();
                    foreach (var stackFrame in s)
                    {
                        var m = stackFrame.GetMethod()!;
                        Console.WriteLine($"{m.Module,-28} {m.DeclaringType,-50} {m.Name}");
                    }
                }, TaskContinuationOptions.AttachedToParent);

                //5
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId.ToString()}");

                //TaskGuidanceTest.dll         TaskGuidanceTest.Program+ChildTaskTest+<>c         <ChildTask>b__1_1
                //System.Private.CoreLib.dll   System.Threading.Tasks.ContinuationTaskFromTask    InnerInvoke
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task+<>c                    <.cctor>b__277_0
                //System.Private.CoreLib.dll   System.Threading.ExecutionContext                  RunFromThreadPoolDispatchLoop
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                        ExecuteWithThreadLocal
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                        ExecuteEntryUnsafe
                //System.Private.CoreLib.dll   System.Threading.Tasks.Task                        ExecuteFromThreadPool
                //System.Private.CoreLib.dll   System.Threading.ThreadPoolWorkQueue               Dispatch
                //System.Private.CoreLib.dll   System.Threading._ThreadPoolWaitCallback           PerformWaitCallback                                                               PerformWaitCallback
            }

            public static async Task<int> GenericTask()
            {
                Console.WriteLine("GenericTask");
                return await Task.FromResult(3);
            }

            public static async ValueTask<int> ValueTask()
            {
                return await System.Threading.Tasks.ValueTask.FromResult(3);
            }
        }

        private class SyncTaskTest
        {
            public static Task SyncTask()
            {
                Console.WriteLine("test");
                return Task.CompletedTask;
            }

            public static Task<int> SyncGenericTask()
            {
                return Task.FromResult(3);
            }

            public static ValueTask<int> SyncValueTask()
            {
                return ValueTask.FromResult(3);
            }
        }
    }
}
