using System;
using System.Threading.Tasks;

namespace ThreadingTest;

public static class TaskFactoryTest
{

    #region Constants & Statics

    public static async Task StartNewUnwrappedTest1()
    {
        static Task<int> ReturnIntFunction()
        {
            return Task.FromResult(42);
        }

        var result = await Task.Factory.StartNewUnwrapped(ReturnIntFunction);

        Console.WriteLine(result);
    }

    public static async Task StartNewUnwrappedTest2()
    {
        static async Task<int> TaskFunction(object? i)
        {
            await Task.Yield();
            return 32;
        }

        var result = await Task.Factory.StartNewUnwrapped(TaskFunction, "32");
        Console.WriteLine(result);
    }

    #endregion

}
