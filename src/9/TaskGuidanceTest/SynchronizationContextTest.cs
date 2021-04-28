using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    partial class Program
    {
        public class SynchronizationContextTest
        {
            public static void Test()
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                //output: SynchronizationContext
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");

                var t = AsyncContinuation_GetContextTest();
                var awaiter = t.GetAwaiter();
                awaiter.GetResult();//阻塞主线程
            }

            public static async Task GetContextTest()
            {
                Console.WriteLine($"GetContextTest {Thread.CurrentThread.ManagedThreadId}");
                await Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId}");
                    //output: null
                    Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                }, TaskCreationOptions.LongRunning).ConfigureAwait(false);
                //output: null
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
            }

            public static async Task AsyncContinuation_GetContextTest()
            {
                //ManagedThreadId 1
                Console.WriteLine($"GetContextTest {Thread.CurrentThread.ManagedThreadId}");

                //continuation 默认跟随 Task 同步执行，
                //RunContinuationsAsynchronously 使 continuation 异步执行
                await Task.Factory.StartNew(() =>
                {
                    //ManagedThreadId 4
                    Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId}");
                    //output: null
                    Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                }, TaskCreationOptions.RunContinuationsAsynchronously).ConfigureAwait(false);
                //output: null
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                //ManagedThreadId 5
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
            }

            public static async Task DirectReturn_GetContextTest()
            {
                Console.WriteLine($"DirectReturn_GetContextTest {Thread.CurrentThread.ManagedThreadId}");

                //等待 Task 之前已经生成结果，Result 会被调用线程同步执行
                await Task.FromResult(1).ConfigureAwait(false);
                //output: SynchronizationContext
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
            }

            public static async Task DirectReturnAndContinue_GetContextTest()
            {
                Console.WriteLine($"DirectReturnAndContinue_GetContextTest {Thread.CurrentThread.ManagedThreadId}");

                await Task.FromResult(1)
                    .ContinueWith(_ =>
                    {
                        Console.WriteLine($"continue task {Thread.CurrentThread.ManagedThreadId}");
                        //output: null
                        Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                    })
                    .ConfigureAwait(false);
                //output: null
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
            }

            public static async Task DirectReturnAndSyncContinue_GetContextTest()
            {
                Console.WriteLine($"DirectReturnAndSynchroContinue_GetContextTest {Thread.CurrentThread.ManagedThreadId}");

                //ContinueWith 默认异步执行，这里将 continuation 附加到父任务所在线程执行
                await Task.FromResult(1)
                    .ContinueWith(_ =>
                        {
                            Console.WriteLine($"continue task {Thread.CurrentThread.ManagedThreadId}");
                            //output: SynchronizationContext
                            Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                        },
                        TaskContinuationOptions.ExecuteSynchronously)
                    .ConfigureAwait(false);
                //output: SynchronizationContext
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}