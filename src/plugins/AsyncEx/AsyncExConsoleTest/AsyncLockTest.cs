using System;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsyncExConsoleTest
{
    internal class AsyncLockTest
    {
        public static void AsyncLock_UsedSync_Test()
        {
            var count = 0;
            var asyncLock = new AsyncLock();

            var tasks = new Task[10000];
            for (int i = 0; i < tasks.Length; i++)
            {
                var task = Task.Factory.StartNew(() =>
                {
                    using (asyncLock.Lock())
                    {
                        return count += 1;
                    }
                });
                tasks[i] = task;
            }

            Task.WaitAll(tasks);

            Console.WriteLine($"result is {count}, must equals {tasks.Length}");
        }

        public static async Task AsyncLock_UsedAsync_Test()
        {
            var count = 0;
            var asyncLock = new AsyncLock();

            var tasks = new Task[10000];
            for (int i = 0; i < tasks.Length; i++)
            {
                var task = Task.Factory.StartNew(async () =>
                {
                    using (await asyncLock.LockAsync())
                    {
                        return count += 1;
                    }
                }).Unwrap();
                tasks[i] = task;
            }

            await tasks.WhenAll();

            Console.WriteLine($"result is {count}, must equals {tasks.Length}");
        }

        public static async Task AsyncLock_UsedAsync_WithMethod_Test()
        {
            var count = 0;
            var asyncLock = new AsyncLock();

            var tasks = new Task[10000];
            for (int i = 0; i < tasks.Length; i++)
            {
                var task = Fun();
                tasks[i] = task;
            }

            await tasks.WhenAll();

            Console.WriteLine($"result is {count}, must equals {tasks.Length}");

            async Task<int> Fun()
            {
                using (await asyncLock.LockAsync())
                {
                    return count += 1;
                }
            }
        }
    }
}
