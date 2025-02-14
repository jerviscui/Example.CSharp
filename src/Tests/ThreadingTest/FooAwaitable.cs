using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ThreadingTest;

[AsyncMethodBuilder(typeof(FooAsyncMethodBuilder<>))]
public sealed class FooAwaitable<TResult>
{
    // 回调，简化起见，未将其包裹到 TaskContinuation 这样的容器里
    private Action? _continuation;

    private Exception? _exception;

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

    internal void TrySetException(Exception exception)
    {
        _exception = exception;
        _completed = true;
        _continuation?.Invoke();
    }

    internal void TrySetResult(TResult result)
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
            if (_fooAwaitable._exception != null)
            {
                // 4. 如果 awaitable 中有异常，则抛出
                throw _fooAwaitable._exception;
            }

            Console.WriteLine("FooAwaiter.GetResult");

            Debug.Assert(_fooAwaitable._result != null, $"{nameof(_result)} is null.");
            return _fooAwaitable._result;
        }

        #endregion
    }
}

[SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "<Pending>")]
public struct FooAsyncMethodBuilder<TResult>
{
    private FooAwaitable<TResult> _awaitable;

    #region Constants & Statics

    // 2. 定义 Create 方法
    public static FooAsyncMethodBuilder<TResult> Create()
    {
        Console.WriteLine("FooAsyncMethodBuilder.Create");
        var awaitable = new FooAwaitable<TResult>();
        var builder = new FooAsyncMethodBuilder<TResult> { _awaitable = awaitable, };
        return builder;
    }

    #endregion

    #region Properties

    // 1. 定义 Task 属性
    public FooAwaitable<TResult> Task
    {
        get
        {
            Console.WriteLine("FooAsyncMethodBuilder.Task");
            return _awaitable;
        }
    }

    #endregion

    #region Methods

    // 4. 定义 AwaitOnCompleted/AwaitUnsafeOnCompleted 方法

    // 如果 awaiter 实现了 INotifyCompletion 接口，就调用 AwaitOnCompleted 方法
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        Console.WriteLine("FooAsyncMethodBuilder.AwaitOnCompleted");
        awaiter.OnCompleted(stateMachine.MoveNext);
    }

    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        Console.WriteLine("FooAsyncMethodBuilder.AwaitUnsafeOnCompleted");
        awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
    }

    public void SetException(Exception exception)
    {
        Console.WriteLine("FooAsyncMethodBuilder.SetException");
        _awaitable.TrySetException(exception);
    }

    // 5. 定义 SetResult/SetException 方法
    public void SetResult(TResult result)
    {
        Console.WriteLine("FooAsyncMethodBuilder.SetResult");
        _awaitable.TrySetResult(result);
    }

    // 6. 定义 SetStateMachine 方法，虽然编译器不会调用，但是编译器要求必须有这个方法
    public void SetStateMachine(IAsyncStateMachine stateMachine)
    {
        Console.WriteLine("FooAsyncMethodBuilder.SetStateMachine");
    }

    // 3. 定义 Start 方法
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        Console.WriteLine("FooAsyncMethodBuilder.Start");
        stateMachine.MoveNext();
    }

    #endregion

}
