using System;
using System.Threading.Tasks;

namespace ThreadingTest;

internal static class TaskAwaiterTest
{

    #region Constants & Statics

    public static async Task OnCompleted_TestAsync()
    {
        _ = Task.Run(() => "Hello").ContinueWith(t => Print(t.Result));
        // 等效于
        var result = await Task.Run(() => "Hello");
        Print(result);

        var awaiter2 = Task.Run(() => "Hello").GetAwaiter();
        awaiter2.OnCompleted(() => Print(awaiter2.GetResult()));

        static void Print(string str)
        {
            Console.WriteLine($"{str} World");
        }
    }

    #endregion

}
