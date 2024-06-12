using System;
using System.Threading;

namespace ThreadingTest;

internal sealed class CancellationTokenTest
{

    #region Constants & Statics

    public static void Cancel_Test()
    {
        using var cts = new CancellationTokenSource();
        Console.WriteLine($"start: {Environment.CurrentManagedThreadId}");

        _ = cts.Token
            .Register(() =>
            {
                Console.WriteLine($"callback: {Environment.CurrentManagedThreadId}");
                Thread.Sleep(5000);
            });

        Console.WriteLine($"Canceling: {Environment.CurrentManagedThreadId}");
        cts.Cancel();
        Console.WriteLine($"Canceled: {Environment.CurrentManagedThreadId}");
    }

    #endregion

}
