using System;
using System.Threading.Tasks;

namespace ThreadingTest;

internal static class TaskAwaiterTest
{

    #region Constants & Statics

    public static void OnCompleted_Test()
    {
        var awaiter1 = Task.Run(() => Console.WriteLine("Hello")).GetAwaiter();
        awaiter1.OnCompleted(() => Console.WriteLine("World"));

        var awaiter2 = Task.Run(() => "Hello").GetAwaiter();
        awaiter2.OnCompleted(() => Console.WriteLine($"{awaiter2.GetResult()} World"));
    }

    #endregion

}
