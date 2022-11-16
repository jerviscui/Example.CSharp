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
}
