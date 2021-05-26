using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace TaskGuidanceTest
{
    partial class Program
    {
        public class ChildTaskTest
        {
            public static void Test()
            {
                var t = AttachedChildTaskAndContinueTest();
                var awaiter = t.GetAwaiter();
                awaiter.GetResult();//阻塞主线程
            }
            
            public static async Task AttachedChildTask()
            {
                //1
                Console.WriteLine($"AttachedChildTask {Thread.CurrentThread.ManagedThreadId}");

                await Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId}");

                    for (int ctr = 0; ctr < 10; ctr++)
                    {
                        int taskNo = ctr;
                        Task.Factory.StartNew((x) =>
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine($"Attached child #{x} completed. {Thread.CurrentThread.ManagedThreadId}");
                        }, taskNo, TaskCreationOptions.AttachedToParent);
                    }
                });

                //wait for all Attached Children
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
            }

            public static async Task DetachedChildTask()
            {
                //1
                Console.WriteLine($"DetachedChildTask {Thread.CurrentThread.ManagedThreadId}");

                await Task.Factory.StartNew(() =>
                {
                    Console.WriteLine($"task {Thread.CurrentThread.ManagedThreadId}");

                    for (int ctr = 0; ctr < 10; ctr++)
                    {
                        int taskNo = ctr;
                        Task.Factory.StartNew((x) =>
                        {
                            Thread.Sleep(1000);
                            Console.WriteLine($"Detached child #{x} completed. {Thread.CurrentThread.ManagedThreadId}");
                        }, taskNo);
                    }
                });

                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");

                //wait for all Detached Children
                await Task.Delay(1000 * 5);
            }

            public static async Task AttachedChildTaskAndContinueTest()
            {
                //1
                Console.WriteLine($"DirectReturnAndContinue_GetContextTest {Thread.CurrentThread.ManagedThreadId}");

                await Task.Factory.StartNew(() =>
                {
                    //4
                    Console.WriteLine($"parent task {Thread.CurrentThread.ManagedThreadId}");

                    Task.Factory.StartNew(() =>
                    {
                        //5
                        Console.WriteLine($"attached child {Thread.CurrentThread.ManagedThreadId}");
                    }, TaskCreationOptions.AttachedToParent);
                }).ContinueWith(_ =>
                {
                    //5
                    Console.WriteLine($"continue task {Thread.CurrentThread.ManagedThreadId}");
                    Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                }).ConfigureAwait(false);

                //5
                Console.WriteLine(SynchronizationContext.Current?.GetType().Name ?? "null");
                Console.WriteLine($"Completed {Thread.CurrentThread.ManagedThreadId}");
            }
        }
    }
}