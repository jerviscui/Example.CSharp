using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest;

public class AsyncLocalTest
{
    private readonly AsyncLocal<string> _asyncLocalString;
    private readonly AsyncLocal<Wrapper<string?>> _asyncLocalWrapper;

    public AsyncLocalTest()
    {
        _asyncLocalString = new AsyncLocal<string>();
        _asyncLocalWrapper = new AsyncLocal<Wrapper<string?>> { Value = new Wrapper<string?>(null) };
    }

    #region Methods

    public async Task AsyncLocal_TestAsync(CancellationToken cancellationToken = default)
    {
        // 设置 AsyncLocal 的值
        _asyncLocalString.Value = "主线程的值";
        Console.WriteLine($"主线程中 AsyncLocal 的值: {_asyncLocalString.Value}");

        // 启动一个异步任务
        await Task.Run(
            async () =>
            {
                Console.WriteLine($"异步任务开始时 AsyncLocal 的值: {_asyncLocalString.Value}");
                // 在异步任务中修改 AsyncLocal 的值
                _asyncLocalString.Value = "异步任务的值";
                Console.WriteLine($"异步任务中 AsyncLocal 的值: {_asyncLocalString.Value}");

                // 启动另一个异步任务
                await Task.Run(
                    () =>
                    {
                        Console.WriteLine($"嵌套异步任务中 AsyncLocal 的值: {_asyncLocalString.Value}");
                    });
            },
            cancellationToken);

        // 在主线程中再次访问 AsyncLocal 的值
        Console.WriteLine($"主线程中 AsyncLocal 的值: {_asyncLocalString.Value}");
    }

    public async Task Wrapper_TestAsync(CancellationToken cancellationToken = default)
    {
        Debug.Assert(_asyncLocalWrapper.Value != null, "AsyncLocal wrapper should not be null");

        _asyncLocalWrapper.Value.Data = "主线程的值";
        Console.WriteLine($"主线程中 AsyncLocal 的值: {_asyncLocalWrapper.Value.Data}");

        await Task.Run(
            async () =>
            {
                Console.WriteLine($"异步任务开始时 AsyncLocal 的值: {_asyncLocalWrapper.Value.Data}");
                _asyncLocalWrapper.Value.Data = "异步任务的值";
                Console.WriteLine($"异步任务中 AsyncLocal 的值: {_asyncLocalWrapper.Value.Data}");

                await Task.Run(
                    () =>
                    {
                        Console.WriteLine($"嵌套异步任务中 AsyncLocal 的值: {_asyncLocalWrapper.Value.Data}");
                    });
            },
            cancellationToken);

        Console.WriteLine($"主线程中 AsyncLocal 的值: {_asyncLocalWrapper.Value.Data}");
    }

    #endregion

    internal class Wrapper<T>
    {
        public Wrapper(T value)
        {
            Data = value;
        }

        #region Properties

        public T Data { get; set; }

        #endregion
    }
}
