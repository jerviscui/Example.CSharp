using System.Threading.Tasks;

namespace ThreadingTest;

internal static class Program
{

    #region Constants & Statics

    private static async Task Main(string[] args)
    {
        // TaskDelayTest.Test();

        // TaskDelayTest.Exception_Test();
        // TaskDelayTest.Exception_Catch_Test();

        // ReaderWriterLockTest.ReaderWriterLock_Test();
        // ReaderWriterLockTest.UpgradeToWriterLock_Test();
        // ReaderWriterLockTest.ReleaseLock_Test();

        // ReaderWriterLockSlimTest.TryEnterReadLock_Test();
        // ReaderWriterLockSlimTest.EnterUpgradeableReadLock_WhenHasRead_Test();
        // ReaderWriterLockSlimTest.EnterUpgradeableReadLock_WhenHasWrite_Test();
        // ReaderWriterLockSlimTest.ExitUpgradeableReadLock_Test();
        // ReaderWriterLockSlimTest.EnterUpgradeableReadLock_OnlyOne_Test();
        // ReaderWriterLockSlimTest.Upgradeable_ToRead_Test();

        // ChannelTest.Reader_Test();

        // CancellationTokenTest.Cancel_Test();

        // InterruptTest.Sleeping_Interrupt_Test();

        // ThreadPoolTest.Enqueue_Test();
        // ThreadPoolTest.Starvation_WaitThread_Test();
        // ThreadPoolTest.Starvation_SetMinThreads_Test();

        // ThreadPoolTest.Starvation_UseGlobalQueue_Test1();
        // ThreadPoolTest.Starvation_UseThreadLocalQueue_Test2();
        // ThreadPoolTest.Starvation_UseGlobalQueue_Test3();

        // ThreadPoolTest.WithoutDelay_UseGlobalQueue_Test1();

        // ThreadPoolTest.SetMinThreads_UseGlobalQueue_Test1();

        // TaskRunTest.RunWithThrow_NoWait(CancellationToken.None);
        // TaskRunTest.RunWithThrow_NoWait_Continue(CancellationToken.None);
        // await TaskRunTest.RunWithThrow_Async(CancellationToken.None);
        // await TaskRunTest.RunWithThrow_Async_ContinueAsync(CancellationToken.None);
        //TaskRunTest.CallAsync_NoWait(CancellationToken.None);

        //await TaskAwaiterTest.OnCompleted_TestAsync();

        //await FooAwaitableTest.Await_TestAsync();
        //var s = await FooAwaitableTest.ReturnType_SyncMehtod_TestAsync();
        //Console.WriteLine(s);
        //var ss = await FooAwaitableTest.ReturnType_AsyncMehtod_TestAsync();
        //Console.WriteLine(ss);

        //using var cts = new CancellationTokenSource(10_000);
        //while (!cts.IsCancellationRequested)
        //{
        //    await Task.Delay(1000);
        //    Console.WriteLine($"ThreadCount: {ThreadPool.ThreadCount}");
        //}

        //await new AsyncLocalTest().AsyncLocal_TestAsync(default);
        //Console.WriteLine();
        //await new AsyncLocalTest().Wrapper_TestAsync(default);

        await TaskFactoryTest.StartNewUnwrappedTest1();
        await TaskFactoryTest.StartNewUnwrappedTest2();
    }

    #endregion

}
