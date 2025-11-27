using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest;

public static class CancellationTokenTest
{

    #region Constants & Statics

    public static async Task Cancel_OperationCanceledException_Test()
    {
        using var cts = new CancellationTokenSource();
        var cancellToken = cts.Token;

        var task = Task.Factory
            .StartNew(
                () =>
                {
                    Thread.Sleep(1000);
                    cancellToken.ThrowIfCancellationRequested();
                    Trace.WriteLine("end");
                },
                cancellToken);

        await Task.Delay(500);
        cts.Cancel();

        try
        {
            await task; // throw OperationCanceledException

            //task.Wait(); // throw AggregateException TaskCanceledException
            //Task.WaitAll(new[] { task }); // throw AggregateException TaskCanceledException

            //await Task.WhenAll(task);// throw OperationCanceledException
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("OperationCanceledException");
        }
        catch (AggregateException exception)
        {
            foreach (var inner in exception.InnerExceptions)
            {
                if (inner is TaskCanceledException)
                {
                    Console.WriteLine("AggregateException TaskCanceledException");
                }
            }
        }
    }

    public static async Task Cancel_TaskCanceledException_Test()
    {
        using var cts = new CancellationTokenSource();
        var cancellToken = cts.Token;

        cts.Cancel(true);

        var task = Task.Factory
            .StartNew(
                () =>
                {
                    Thread.Sleep(1000);
                    cancellToken.ThrowIfCancellationRequested();
                    Trace.WriteLine("end");
                },
                cancellToken);

        try
        {
            await task; // throw TaskCanceledException

            //task.Wait(); // throw AggregateException TaskCanceledException
            //Task.WaitAll(new[] { task }); // throw AggregateException TaskCanceledException
        }
        catch (TaskCanceledException)
        {
            Console.WriteLine("TaskCanceledException");
        }
        catch (AggregateException exception)
        {
            foreach (var inner in exception.InnerExceptions)
            {
                if (inner is TaskCanceledException)
                {
                    Console.WriteLine("AggregateException TaskCanceledException");
                }
            }
        }
    }

    [SuppressMessage("Usage", "CA2201:Do not raise reserved exception types", Justification = "<Pending>")]
    public static async Task Cancel_ThrowFirst_Test()
    {
        using var cts = new CancellationTokenSource();
        Console.WriteLine($"start: {Environment.CurrentManagedThreadId}");

        var token = cts.Token;

        var t1 = Task.Run(
            async () =>
            {
                Console.WriteLine($"run: 1-{Environment.CurrentManagedThreadId}");
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                }
                token.ThrowIfCancellationRequested();
            });

        var t2 = Task.Run(
            async () =>
            {
                Console.WriteLine($"run: 2-{Environment.CurrentManagedThreadId}");
                while (!token.IsCancellationRequested)
                {
                    await Task.Delay(100);
                }
                token.ThrowIfCancellationRequested();
            });

        _ = cts.Token
            .Register(
                () =>
                {
                    Console.WriteLine($"callback: 1-{Environment.CurrentManagedThreadId}");

                    throw new Exception("1");
                });

        _ = cts.Token
            .Register(
                () =>
                {
                    Console.WriteLine($"callback: 2-{Environment.CurrentManagedThreadId}");
                    throw new Exception("2");
                });

        await Task.Delay(300);

        cts.Cancel(true);
        Console.WriteLine($"Canceled: {Environment.CurrentManagedThreadId}");

        await Task.WhenAll(t1, t2);
    }

    public static void Register_Test()
    {
        using var cts = new CancellationTokenSource();
        Console.WriteLine($"start: {Environment.CurrentManagedThreadId}");

        _ = cts.Token
            .Register(
                () =>
                {
                    Console.WriteLine($"callback: {Environment.CurrentManagedThreadId}");
                    Thread.Sleep(5000);
                });

        Console.WriteLine($"Canceling: {Environment.CurrentManagedThreadId}");
        cts.Cancel();
        Console.WriteLine($"Canceled: {Environment.CurrentManagedThreadId}");
    }

    public static void Register_Twice_Test()
    {
        using var cts = new CancellationTokenSource();
        Console.WriteLine($"start: {Environment.CurrentManagedThreadId}");

        _ = cts.Token
            .Register(
                () =>
                {
                    Console.WriteLine($"callback: 1-{Environment.CurrentManagedThreadId}");
                    Thread.Sleep(1000);
                });

        _ = cts.Token
            .Register(
                () =>
                {
                    Console.WriteLine($"callback: 2-{Environment.CurrentManagedThreadId}");
                    Thread.Sleep(1000);
                });

        Console.WriteLine($"Canceling: {Environment.CurrentManagedThreadId}");
        cts.Cancel();
        Console.WriteLine($"Canceled: {Environment.CurrentManagedThreadId}");
    }

    public static void Register_Twice_ThrowFirst_Test()
    {
        using var cts = new CancellationTokenSource();
        Console.WriteLine($"start: {Environment.CurrentManagedThreadId}");

        _ = cts.Token
            .Register(
                () =>
                {
                    Console.WriteLine($"callback: 1-{Environment.CurrentManagedThreadId}");
                    Thread.Sleep(1000);
                });

        _ = cts.Token
            .Register(
                () =>
                {
                    Console.WriteLine($"callback: 2-{Environment.CurrentManagedThreadId}");
                    Thread.Sleep(1000);
                });

        Console.WriteLine($"Canceling: {Environment.CurrentManagedThreadId}");
        cts.Cancel(false);
        Console.WriteLine($"Canceled: {Environment.CurrentManagedThreadId}");
    }

    #endregion

}
