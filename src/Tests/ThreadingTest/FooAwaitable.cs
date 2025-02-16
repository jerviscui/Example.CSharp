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

[SuppressMessage("Members", "CRR0026:Unused member", Justification = "<Pending>")]
// must public
public class FooAsyncMethodBuilder<TResult>
{

    #region Constants & Statics

    // 1. 创建一个 AsyncTaskMethodBuilder
#pragma warning disable CA1000 // Do not declare static members on generic types
    public static FooAsyncMethodBuilder<TResult> Create()
#pragma warning restore CA1000 // Do not declare static members on generic types
    {
        Console.WriteLine("FooAsyncMethodBuilder.Create");
        var awaitable = new FooAwaitable<TResult>();
        var builder = new FooAsyncMethodBuilder<TResult>(awaitable);

        return builder;
    }

    #endregion

    private readonly FooAwaitable<TResult> _awaitable;

    public FooAsyncMethodBuilder(FooAwaitable<TResult> awaitable)
    {
        _awaitable = awaitable;
    }

    #region Properties

    // 5. 作为 async 方法的返回值
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

    // 3. 将状态机的 MoveNext 方法注册为 async 方法内 await 的 Task 的回调
    public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : INotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        Console.WriteLine("FooAsyncMethodBuilder.AwaitOnCompleted");
        awaiter.OnCompleted(stateMachine.MoveNext);
    }

    // 4. 同 AwaitOnCompleted，但是清空 ExecutionContext
    public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
        where TAwaiter : ICriticalNotifyCompletion
        where TStateMachine : IAsyncStateMachine
    {
        Console.WriteLine("FooAsyncMethodBuilder.AwaitUnsafeOnCompleted");
        awaiter.UnsafeOnCompleted(stateMachine.MoveNext);
    }

    // 7. SetException
    public void SetException(Exception exception)
    {
        Console.WriteLine("FooAsyncMethodBuilder.SetException");
        _awaitable.TrySetException(exception);
    }

    // 8. SetResult
    public void SetResult(TResult result)
    {
        Console.WriteLine("FooAsyncMethodBuilder.SetResult");
        _awaitable.TrySetResult(result);
    }

    // 6. 绑定状态机，但编译器的编译结果不会调用
#pragma warning disable IDE0060 // Remove unused parameter
    public void SetStateMachine(IAsyncStateMachine stateMachine)
#pragma warning restore IDE0060 // Remove unused parameter
    {
        Console.WriteLine("FooAsyncMethodBuilder.SetStateMachine");
    }

    // 2. 开始执行 AsyncTaskMethodBuilder 及其绑定的状态机
    public void Start<TStateMachine>(ref TStateMachine stateMachine)
        where TStateMachine : IAsyncStateMachine
    {
        Console.WriteLine("FooAsyncMethodBuilder.Start");
        stateMachine.MoveNext();
    }

    #endregion

}
