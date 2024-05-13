using System;
using System.Globalization;
using System.Linq.Expressions;

namespace CodeAnalysisTest;

internal class CodeRushTestBase
{
    public virtual void MethodName()
    {
    }
}

internal sealed class CodeRushTest : CodeRushTestBase
{
    public override void MethodName()
    {
        var b = Random.Shared.Next() > 1;
        // CRR0008
        if (b)
        {
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine();
        }

        // CRR0009
        if (b)
        {
            Console.WriteLine();
        }

#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        _ = DemoMethod30(CancellationToken.None);
        _ = DemoMethod2(CancellationToken.None);

        OnDemoMethod33Async(CancellationToken.None);

        _ = ProcessFile35Async("");

        //CRR0039 - The 'await' expression without cancellation token
        _ = DemoMethod36Async();

        _ = MethodName37Async(1, CancellationToken.None);
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
    }

    //CRR0034 - The asynchronous method should contain the "Async" suffix
    public static async Task<string> DemoMethod30(CancellationToken token = default)
    {
        //CRR0030 - Redundant 'await'
        return await Task.Run(() => "result2", token).ConfigureAwait(false);
    }
    public static Task<string> DemoMethod2(CancellationToken token = default)
    {
        return Task.Run(() => "result2", token);
    }

    //CRR0033 - The void async method should be in a try/catch block
    public static async void OnDemoMethod33Async(CancellationToken token)
    {
        await Task.Run(Format, token).ConfigureAwait(false);
    }

    //CRR0035 - No CancellationToken parameter in the asynchronous method
    private static async Task ProcessFile35Async(string path)
    {
        //CRR0039 - The 'await' expression without cancellation token
        var lines = await File.ReadAllLinesAsync(path).ConfigureAwait(false);
        foreach (var line in lines)
        {
            //...
        }
    }

    private static async Task<string> DemoMethod36Async(CancellationToken token = default)
    {
        //CRR0036 - The 'await Task.FromResult()' expression is redundant
        return await Task.FromResult("Success");
    }

    public static async Task<string> MethodName37Async(int value, CancellationToken token)
    {
        token.ThrowIfCancellationRequested();

        //CRR0037 - Task.Wait is used in an async method
        Thread.Sleep(10);

        _ = await DemoMethod36Async(token);

        return value.ToString(CultureInfo.CurrentCulture);

    }

    public static void Format()
    {
        var letters = new string[] { "A", "B", "C", "D", "E", "F", "A", "B", "C", "D", "E", "F", "A", "B", "C", "D", "E", "F", "A", "B", "C", "D", "E", "F" };

        var digits = new List<int> { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

    }
}
