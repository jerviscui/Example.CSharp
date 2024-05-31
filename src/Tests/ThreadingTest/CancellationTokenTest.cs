using System;
using System.Threading;

namespace ThreadingTest;

internal sealed class CancellationTokenTest
{
    public static void Cancel_Test()
    {
        var cts = new CancellationTokenSource();
        Console.WriteLine($"start: {Environment.CurrentManagedThreadId}");

        cts.Token.Register(() =>
        {
            Console.WriteLine($"callback: {Environment.CurrentManagedThreadId}");
            Thread.Sleep(5000);
        });

        Console.WriteLine($"Canceling: {Environment.CurrentManagedThreadId}");
        cts.Cancel();
        Console.WriteLine($"Canceled: {Environment.CurrentManagedThreadId}");
    }
}
