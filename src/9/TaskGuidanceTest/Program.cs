using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var t = TaskTest.SimpleTask2();
            var awaiter = t.GetAwaiter();
            awaiter.GetResult();//阻塞主线程

            //SynchronizationContextTest.Test();

            //CultureTest.Test();
        }

        public class TaskTest
        {
            public static async void Void()
            {
                Console.WriteLine("");
            }

            public static async Task SimpleTask()
            {
                Console.WriteLine("SimpleTask");
            }
            
            public static async Task SimpleTask2()
            {
                Console.WriteLine($"SimpleTask2 {Thread.CurrentThread.ManagedThreadId}");
                await Task.Factory.StartNew(() =>
                {
                    Thread.Sleep(1000 * 10);
                    Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId}");
                }, TaskCreationOptions.LongRunning).ConfigureAwait(false);
                //await Task.Delay(1000 * 30).ContinueWith(_ => Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId}"), TaskContinuationOptions.AttachedToParent).ConfigureAwait(false);
                var s = new StackTrace().GetFrames();
                foreach (var stackFrame in s)
                {
                    var m = stackFrame.GetMethod();
                    Console.WriteLine($"{m.Module,-28} {m.DeclaringType,-103} {m.Name}");
                }
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
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

        class SyncTaskTest
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