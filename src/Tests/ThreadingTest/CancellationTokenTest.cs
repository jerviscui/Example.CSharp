using System;
using System.Threading;

namespace ThreadingTest;

internal class CancellationTokenTest
{
    public static void Cancel_Test()
    {
        var cts = new CancellationTokenSource();
        Console.WriteLine($"start: {Thread.CurrentThread.ManagedThreadId}");

        cts.Token.Register(() =>
        {
            Console.WriteLine($"callback: {Thread.CurrentThread.ManagedThreadId}");
            Thread.Sleep(5000);
        });

        Console.WriteLine($"Canceling: {Thread.CurrentThread.ManagedThreadId}");
        cts.Cancel();
        Console.WriteLine($"Canceled: {Thread.CurrentThread.ManagedThreadId}");
    }
}
