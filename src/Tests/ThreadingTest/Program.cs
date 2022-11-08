using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //TaskDelayTest.Test();

            //TaskDelayTest.Exception_Test();
            //TaskDelayTest.Exception_Catch_Test();

            //ReaderWriterLockTest.ReaderWriterLock_Test();
            //ReaderWriterLockTest.UpgradeToWriterLock_Test();
            //ReaderWriterLockTest.ReleaseLock_Test();

            //ReaderWriterLockSlimTest.TryEnterReadLock_Test();
            //ReaderWriterLockSlimTest.EnterUpgradeableReadLock_WhenHasRead_Test();
            //ReaderWriterLockSlimTest.EnterUpgradeableReadLock_WhenHasWrite_Test();
            //ReaderWriterLockSlimTest.ExitUpgradeableReadLock_Test();
            //ReaderWriterLockSlimTest.EnterUpgradeableReadLock_OnlyOne_Test();
            //ReaderWriterLockSlimTest.Upgradeable_ToRead_Test();

            //ChannelTest.Reader_Test();

            //CancellationTokenTest.Cancel_Test();

            //ThreadPoolTest.Enqueue_Test();
            //ThreadPoolTest.Starvation_WaitThread_Test();
            ThreadPoolTest.Starvation_SetMinThreads_Test();

            while (true)
            {
                await Task.Delay(1000);
                Console.WriteLine($"total: {ThreadPool.ThreadCount}");
            }
        }
    }
}
