using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    internal partial class Program
    {
        public class SynchronizationContextTest
        {
            public static void Test()
            {
                SynchronizationContext.SetSynchronizationContext(new SynchronizationContext());
                //output: SynchronizationContext
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");

                var t = GetContextTest2();
                var awaiter = t.GetAwaiter();
                awaiter.GetResult(); //阻塞主线程
            }

            public static async Task GetContextTest2()
            {
                //1
                Console.WriteLine($"GetContextTest2 {Environment.CurrentManagedThreadId.ToString()}");
                await Task.Factory.StartNew(() =>
                {
                    //4
                    Console.WriteLine($"task {Environment.CurrentManagedThreadId.ToString()}");
                    //output: null
                    Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                }).ConfigureAwait(false);
                //output: null
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                //4
                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");
            }

            public static async Task GetContextTest()
            {
                //1
                Console.WriteLine($"GetContextTest {Environment.CurrentManagedThreadId.ToString()}");
                await Task.Factory.StartNew(() =>
                {
                    //7
                    Console.WriteLine($"task {Environment.CurrentManagedThreadId.ToString()}");
                    //output: null
                    Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                }, TaskCreationOptions.LongRunning).ConfigureAwait(false);
                //output: null

                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                //4 or 7
                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");
            }

            public static async Task AsyncContinuation_GetContextTest()
            {
                //ManagedThreadId 1
                Console.WriteLine($"AsyncContinuation_GetContextTest {Environment.CurrentManagedThreadId.ToString()}");

                //continuation 默认跟随 Task 同步执行，
                //RunContinuationsAsynchronously 使 continuation 异步执行
                await Task.Factory.StartNew(() =>
                {
                    //ManagedThreadId 4
                    Console.WriteLine($"task {Environment.CurrentManagedThreadId.ToString()}");
                    //output: null
                    Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                }, TaskCreationOptions.RunContinuationsAsynchronously).ConfigureAwait(false);
                //output: null
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                //ManagedThreadId 5 or 4
                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");
            }

            public static async Task DirectReturn_GetContextTest()
            {
                //1
                Console.WriteLine($"DirectReturn_GetContextTest {Environment.CurrentManagedThreadId.ToString()}");

                //等待 Task 之前已经生成结果，Result 会被调用线程同步执行
                await Task.FromResult(1).ConfigureAwait(false);
                //output: SynchronizationContext
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                //1
                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");
            }

            public static async Task DirectReturnAndContinue_GetContextTest()
            {
                //1
                Console.WriteLine($"DirectReturnAndContinue_GetContextTest {Environment.CurrentManagedThreadId.ToString()}");

                await Task.FromResult(1)
                    .ContinueWith(_ =>
                    {
                        //4
                        Console.WriteLine($"continue task {Environment.CurrentManagedThreadId.ToString()}");
                        //output: null
                        Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                    })
                    .ConfigureAwait(false);
                //output: null
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                //4
                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");
            }

            public static async Task DirectReturnAndSyncContinue_GetContextTest()
            {
                //1
                Console.WriteLine($"DirectReturnAndSynchroContinue_GetContextTest {Environment.CurrentManagedThreadId.ToString()}");

                //ContinueWith 默认异步执行，这里将 continuation 附加到父任务所在线程执行
                await Task.FromResult(1)
                    .ContinueWith(_ =>
                        {
                            //1
                            Console.WriteLine($"continue task {Environment.CurrentManagedThreadId.ToString()}");
                            //output: SynchronizationContext
                            Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                        },
                        TaskContinuationOptions.ExecuteSynchronously)
                    .ConfigureAwait(false);
                //output: SynchronizationContext
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                //1
                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");
            }
        }
    }
}
