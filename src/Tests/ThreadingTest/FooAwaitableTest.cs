using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest;

public static class FooAwaitableTest
{

    #region Constants & Statics

    public static async Task AwaitTestAsync()
    {
        var fooAwaitable = new FooAwaitable<string>();

        fooAwaitable.Run(
            () =>
            {
                // 可以把Sleep去掉看看
                Thread.Sleep(100);
                Console.WriteLine("Hello");
                return "World";
            });

        var x = await fooAwaitable;
        Console.WriteLine(x);

        //output:
        //FooAwaiter.OnCompleted
        //FooAwaiter.OnCompleted: added continuation
        //Hello
        //FooAwaiter.GetResult
        //World
    }

    // error CS1983: The return type of an async method must be void, Task, Task<T>, a task-like type, IAsyncEnumerable<T>, or IAsyncEnumerator<T>
    public static async FooAwaitable<string> ReturnType_AsyncMehtod_TestAsync()
    {
        await Task.Delay(10);

        //var fooAwaitable = new FooAwaitable<string>();

        return "Hi World!";

        //output:
        //FooAsyncMethodBuilder.Create
        //FooAsyncMethodBuilder.Start
        //FooAsyncMethodBuilder.AwaitUnsafeOnCompleted
        //FooAsyncMethodBuilder.Task
        //FooAwaiter.OnCompleted
        //FooAwaiter.OnCompleted: added continuation
        //FooAsyncMethodBuilder.SetResult
        //FooAwaiter.GetResult
        //Hi World!
    }

    public static FooAwaitable<string> ReturnType_SyncMehtod_TestAsync()
    {
        var fooAwaitable = new FooAwaitable<string>();

        fooAwaitable.Run(
            () =>
            {
                // 可以把Sleep去掉看看
                Thread.Sleep(100);
                Console.WriteLine("Hello");
                return "World";
            });

        return fooAwaitable;
    }

    #endregion

}
