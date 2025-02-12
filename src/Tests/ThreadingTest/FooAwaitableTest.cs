using System;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest;

public static class FooAwaitableTest
{

    #region Constants & Statics

    public static async Task Await_TestAsync()
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

    #endregion

}
