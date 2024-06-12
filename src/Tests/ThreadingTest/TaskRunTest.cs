using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest;

internal static class TaskRunTest
{

    #region Constants & Statics

    public static void CallAsync_NoWait(CancellationToken cancellationToken = default)
    {
        // don't crash!

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _ = RunWithThrow_Async(cancellationToken);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    public static async Task RunWithThrow_Async(CancellationToken cancellationToken = default)
    {
        // program crash!
        // Unhandled exception.

        var t = Task.Run(
            () =>
            {
                Console.WriteLine("RunWithThrow_Async");
                throw new NotSupportedException("throw internal Task.Run");
            },
            cancellationToken);

        await t;
    }

    public static async Task RunWithThrow_Async_ContinueAsync(CancellationToken cancellationToken = default)
    {
        // don't crash!

        var t = Task
            .Run(
                () =>
                {
                    Console.WriteLine("RunWithThrow_Async");
                    throw new NotSupportedException("throw internal Task.Run");
                },
                cancellationToken)
            .ContinueWith(
                (t) =>
                {
                    if (t.IsFaulted)
                    {
                        Console.WriteLine(t.Exception.Message);
                    }
                },
                cancellationToken);

        await t;
    }

    public static void RunWithThrow_NoWait(CancellationToken cancellationToken = default)
    {
        // don't crash!

        _ = Task.Run(
            () =>
            {
                Console.WriteLine("RunWithThrow_NoWait");
                throw new NotSupportedException("throw internal Task.Run");
            },
            cancellationToken);
    }

    public static void RunWithThrow_NoWait_Continue(CancellationToken cancellationToken = default)
    {
        // don't crash!

        _ = Task
            .Run(
                () =>
                {
                    Console.WriteLine("RunWithThrow_NoWait");
                    throw new NotSupportedException("throw internal Task.Run");
                },
                cancellationToken)
            .ContinueWith(
                (t) =>
                {
                    if (t.IsFaulted)
                    {
                        Console.WriteLine(t.Exception.Message);
                    }
                },
                cancellationToken);
    }

    #endregion

}
