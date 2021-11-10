using System;
using System.Threading;
using System.Threading.Tasks;
using Nito.AsyncEx;

namespace AsyncExConsoleTest
{
    internal class AsyncReaderWriterLockTest
    {
        public static void ReaderWriterLock_Test()
        {
            var rwLock = new AsyncReaderWriterLock();

            using var tokenSource = new CancellationTokenSource(30 * 1000);
            var token = tokenSource.Token;

            var count = 0;

            var read1 = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    using (await rwLock.ReaderLockAsync())
                    {
                        await Task.Delay(100);
                        Console.WriteLine($"r1: {count}");
                    }
                }
            }, token);

            var read2 = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    using (await rwLock.ReaderLockAsync())
                    {
                        await Task.Delay(100);
                        Console.WriteLine($"r2: {count}");
                    }
                }
            }, token);

            var write = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    using (await rwLock.WriterLockAsync())
                    {
                        await Task.Delay(5000);
                        count += 1;
                    }

                    await Task.Delay(1000);
                }
            }, token);

            Task.WaitAll(read1, read2, write);
        }
    }
}
