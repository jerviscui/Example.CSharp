using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var t = TaskTest.SimpleTask2();

            var awaiter = t.GetAwaiter();
            awaiter.GetResult();//阻塞主线程
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
                await Task.Delay(1000).ContinueWith(_ => Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId}"));
                var s = new StackTrace().GetFrames();
                foreach (var stackFrame in s)
                {
                    Console.WriteLine(stackFrame.ToString());
                }
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
                //调用堆栈输出
                //MoveNext at offset 754 in file: line: column < filename unknown >:0:0
                //ExecutionContextCallback at offset 21 in file: line: column < filename unknown >:0:0
                //RunInternal at offset 128 in file: line: column < filename unknown >:0:0
                //MoveNext at offset 147 in file: line: column < filename unknown >:0:0
                //MoveNext at offset 12 in file: line: column < filename unknown >:0:0
                //<OutputWaitEtwEvents>b__12_0 at offset 266 in file: line: column < filename unknown >:0:0
                //Invoke at offset 33 in file: line: column < filename unknown >:0:0
                //RunOrScheduleAction at offset 106 in file: line: column < filename unknown >:0:0
                //RunContinuations at offset 213 in file: line: column < filename unknown >:0:0
                //FinishContinuations at offset 54 in file: line: column < filename unknown >:0:0
                //FinishStageThree at offset 47 in file: line: column < filename unknown >:0:0
                //FinishStageTwo at offset 384 in file: line: column < filename unknown >:0:0
                //ExecuteWithThreadLocal at offset 516 in file: line: column < filename unknown >:0:0
                //ExecuteEntryUnsafe at offset 83 in file: line: column < filename unknown >:0:0
                //ExecuteFromThreadPool at offset 10 in file: line: column < filename unknown >:0:0
                //Dispatch at offset 442 in file: line: column < filename unknown >:0:0
                //PerformWaitCallback at offset 10 in file: line: column < filename unknown >:0:0
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