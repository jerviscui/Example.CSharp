using System;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    internal partial class Program
    {
        public class ChildTaskTest
        {
            public static void Test()
            {
                var t = AttachedChildTaskAndContinueTest();
                var awaiter = t.GetAwaiter();
                awaiter.GetResult(); //阻塞主线程
            }

            public static async Task AttachedChildTask()
            {
                //1
                Console.WriteLine($"AttachedChildTask {Environment.CurrentManagedThreadId.ToString()}");

                await Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"task {Environment.CurrentManagedThreadId.ToString()}");

                    for (int ctr = 0; ctr < 10; ctr++)
                    {
                        int taskNo = ctr;
                        Task.Factory.StartNew(x =>
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine(
                                $"Attached child #{x} completed. {Environment.CurrentManagedThreadId.ToString()}");
                        }, taskNo, TaskCreationOptions.AttachedToParent);
                    }
                });

                //wait for all Attached Children
                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");
            }

            public static async Task DetachedChildTask()
            {
                //1
                Console.WriteLine($"DetachedChildTask {Environment.CurrentManagedThreadId.ToString()}");

                await Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"task {Environment.CurrentManagedThreadId.ToString()}");

                    for (int ctr = 0; ctr < 10; ctr++)
                    {
                        int taskNo = ctr;
                        Task.Factory.StartNew(x =>
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine(
                                $"Detached child #{x} completed. {Environment.CurrentManagedThreadId.ToString()}");
                        }, taskNo);
                    }
                });

                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");

                //wait for all Detached Children
                await Task.Delay(1000 * 5);
            }

            public static async Task AttachedChildTaskAndContinueTest()
            {
                //1
                Console.WriteLine(
                    $"DirectReturnAndContinue_GetContextTest {Environment.CurrentManagedThreadId.ToString()}");

                await Task.Factory.StartNew(() =>
                {
                    //4
                    Console.WriteLine($"parent task {Environment.CurrentManagedThreadId.ToString()}");

                    Task.Factory.StartNew(() =>
                    {
                        //5
                        Console.WriteLine($"attached child {Environment.CurrentManagedThreadId.ToString()}");
                    }, TaskCreationOptions.AttachedToParent);
                }).ContinueWith(_ =>
                {
                    //5
                    Console.WriteLine($"continue task {Environment.CurrentManagedThreadId.ToString()}");
                    Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                }).ConfigureAwait(false);

                //5
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                Console.WriteLine($"Completed {Environment.CurrentManagedThreadId.ToString()}");
            }
        }
    }
}
