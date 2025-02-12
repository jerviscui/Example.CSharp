using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ThreadingTest;

internal sealed class FooAwaitable<TResult>
{
    // 回调，简化起见，未将其包裹到 TaskContinuation 这样的容器里
    private Action? _continuation;

    private TResult? _result;

    #region Properties

    private volatile bool _completed;

    public bool IsCompleted => _completed;

    #endregion

    #region Methods

    private bool AddFooContinuation(Action action)
    {
        if (_completed)
        {
            return false;
        }
        _continuation += action;
        return true;
    }

    private void TrySetResult(TResult result)
    {
        _result = result;
        _completed = true;
        _continuation?.Invoke();
    }

    #region awaitable

    // Awaitable 中的关键部分，提供 GetAwaiter 方法
    public FooAwaiter<TResult> GetAwaiter()
    {
        return new FooAwaiter<TResult>(this);
    }

    #endregion

    public void Run(Func<TResult> func)
    {
        new Thread(
            () =>
            {
                var result = func();
                TrySetResult(result);
            })
        {
            IsBackground = true
        }.Start();
    }

    #endregion

    public readonly struct FooAwaiter<T> : INotifyCompletion
    {
        private readonly FooAwaitable<T> _fooAwaitable;

        public FooAwaiter(FooAwaitable<T> fooAwaitable)
        {
            _fooAwaitable = fooAwaitable;
        }

        #region Properties

        // 2. 实现 IsCompleted 属性
        public bool IsCompleted => _fooAwaitable.IsCompleted;

        #endregion

        #region INotifyCompletion implementations

        public void OnCompleted(Action continuation)
        {
            Console.WriteLine("FooAwaiter.OnCompleted");
            if (_fooAwaitable.AddFooContinuation(continuation))
            {
                Console.WriteLine("FooAwaiter.OnCompleted: added continuation");
            }
            else
            {
                // 试着把上面的 Thread.Sleep(100) 删掉看看，就有可能会执行到这里
                // 也就是回调的注册时间有可能晚于任务完成的时间
                Console.WriteLine("FooAwaiter.OnCompleted: already completed, invoking continuation");
                continuation();
            }
        }

        #endregion

        #region Methods

        // 3. 实现 GetResult 方法
        public T GetResult()
        {
            Console.WriteLine("FooAwaiter.GetResult");

            Debug.Assert(_fooAwaitable._result != null, $"{nameof(_result)} is null.");
            return _fooAwaitable._result;
        }

        #endregion
    }
}
