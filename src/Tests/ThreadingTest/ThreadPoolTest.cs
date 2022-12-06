using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest;

public class ThreadPoolTest
{
    public static void Enqueue_Test()
    {
        ThreadPool.QueueUserWorkItem(state =>
        {
            Console.WriteLine(state.ToString());
            Console.WriteLine($"{Environment.CurrentManagedThreadId.ToString()} - {Thread.CurrentThread.Name}");
        }, 1, true);

        ThreadPool.UnsafeQueueUserWorkItem(state =>
        {
            Thread.Sleep(200);
            Console.WriteLine(state.ToString());
            Console.WriteLine($"{Environment.CurrentManagedThreadId.ToString()} - {Thread.CurrentThread.Name}");
        }, 2, true);
    }

    /// <summary>
    /// 线程池饥饿
    /// </summary>
    private static void Starvation_Test()
    {
        var tasks = new Task[250];
        for (int i = 0; i < tasks.Length; i++)
        {
            var i1 = i;
            tasks[i] = Task.Factory.StartNew(() =>
            {
                Thread.Sleep(1000);
                Console.WriteLine($"{i1} total: {ThreadPool.ThreadCount}");
            });
        }

        Task.WaitAll(tasks);
    }

    public static void Starvation_WaitThread_Test()
    {
        //进程内设置无效，需要程序启动前设置环境变量
        //Environment.SetEnvironmentVariable("NUMBER_OF_PROCESSORS", "8");
        //Environment.SetEnvironmentVariable("DOTNET_PROCESSOR_COUNT", "8");
        Console.WriteLine($"processor: {Environment.ProcessorCount}");

        Console.WriteLine($"total: {ThreadPool.ThreadCount}");

        ThreadPool.SetMinThreads(16, 16);
        ThreadPool.GetMinThreads(out var workerThreads, out var completionPortThreads);
        Console.WriteLine($"{workerThreads}  {completionPortThreads}");

        Starvation_Test();
    }

    public static void Starvation_SetMinThreads_Test()
    {
        ThreadPool.SetMinThreads(200, 200);

        Starvation_Test();
    }

    #region Use ThreadLocalQueue

    /// <summary>
    /// 线程数无限增长
    /// </summary>
    public static void Starvation_UseGlobalQueue_Test1()
    {
        ThreadPool.SetMinThreads(8, 8);

        Task.Factory.StartNew(Producer1);
    }

    /// <summary>
    /// 线程数稳定
    /// </summary>
    public static void Starvation_UseThreadLocalQueue_Test2()
    {
        ThreadPool.SetMinThreads(8, 8);

        Task.Factory.StartNew(Producer2);
    }

    /// <summary>
    /// 线程数无限增长
    /// </summary>
    public static void Starvation_UseGlobalQueue_Test3()
    {
        ThreadPool.SetMinThreads(8, 8);

        Task.Factory.StartNew(Producer3);
    }

    private static volatile int _i;

    private static void Producer1()
    {
        var i = 100;
        while (true)
        {
#pragma warning disable CS4014
            Process1();
#pragma warning restore CS4014

            Thread.Sleep(100);
        }
    }

    private static void Producer2()
    {
        while (true)
        {
            Task.Factory.StartNew(Process2); //在线程本地队列创建任务

            Thread.Sleep(100);
        }
    }

    private static void Producer3()
    {
        while (true)
        {
            Task.Factory.StartNew(Process2, TaskCreationOptions.PreferFairness); //在全局队列创建任务

            Thread.Sleep(100);
        }
    }

    private static async Task Process1()
    {
        await Task.Yield(); //做一个异步切换，在全局队列创建任务

        var i = Interlocked.Increment(ref _i);

        var tcs = new TaskCompletionSource<bool>();

#pragma warning disable CS4014
        //在线程本地队列创建任务
        Task.Run(() =>
#pragma warning restore CS4014
        {
            Console.WriteLine($"{i} run");

            Thread.Sleep(3000);
            tcs.SetResult(true);
        });

        //所以此处是当前线程等待自己队列中的任务完成，死锁！
        //何时解除，当其他线程抢占 tcs 任务并执行完成后解除死锁
        //但是，由于新建线程全部去执行全局队列的任务，没有线程去抢占 tcs 任务，所以死锁永远无法解除
        tcs.Task.Wait();

        Console.WriteLine($"{i} completed.");
    }

    private static void Process2()
    {
        var i = Interlocked.Increment(ref _i);

        var tcs = new TaskCompletionSource<bool>();
        //在线程本地队列创建任务
        Task.Run(() =>
        {
            Console.WriteLine($"{i} run");

            Thread.Sleep(3000);
            tcs.SetResult(true);
        });

        //任务全部在线程本地队列中排队，
        //线程池队列长度虽然在增长，但是全局队列中没有任务
        //所以新增线程可以抢占 tcs 任务解除死锁，最终达到动态平衡
        tcs.Task.Wait();

        Console.WriteLine($"{i} completed.");
    }

    #endregion

    #region ThreadsToAddWithoutDelay

    public static void WithoutDelay_UseGlobalQueue_Test1()
    {
        //设置 ThreadsToAddWithoutDelay 依然有线程池饥饿问题
        var data = AppContext.GetData("System.Threading.ThreadPool.Blocking.ThreadsToAddWithoutDelay_ProcCountFactor");
        Console.WriteLine(data);

        Task.Factory.StartNew(Producer1);
    }

    public static void WithoutDelay_UseThreadLocalQueue_Test2()
    {
        var data = AppContext.GetData("System.Threading.ThreadPool.Blocking.ThreadsToAddWithoutDelay_ProcCountFactor");
        Console.WriteLine(data);

        Task.Factory.StartNew(Producer2);
    }

    #endregion

    #region SetMinThreads

    public static void SetMinThreads_UseGlobalQueue_Test1()
    {
        ThreadPool.SetMinThreads(400, 400);

        Task.Factory.StartNew(Producer1);
    }

    public static void SetMinThreads_UseThreadLocalQueue_Test2()
    {
        ThreadPool.SetMinThreads(400, 400);

        Task.Factory.StartNew(Producer2);
    }

    #endregion
}
