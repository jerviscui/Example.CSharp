using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadingTest;

[SuppressMessage("Async/await", "CRR0038:CancellationToken parameter is never used.", Justification = "<Pending>")]
[SuppressMessage(
    "Minor Code Smell",
    "S4261:Methods should be named according to their synchronicities",
    Justification = "<Pending>")]
public static class TaskFactoryExtensions
{

    #region Constants & Statics

    public static Task<TResult> StartNewUnwrapped<TResult>(this TaskFactory factory, Func<Task<TResult>> function)
    {
        var task = factory.StartNew(function);

        return task.Unwrap();
    }

    public static Task<TResult> StartNewUnwrapped<TResult>(
        this TaskFactory factory,
        Func<Task<TResult>> function,
        CancellationToken cancellationToken)
    {
        var task = factory.StartNew(function, cancellationToken);

        return task.Unwrap();
    }

    public static Task<TResult> StartNewUnwrapped<TResult>(
        this TaskFactory factory,
        Func<Task<TResult>> function,
        TaskCreationOptions creationOptions)
    {
        var task = factory.StartNew(function, creationOptions);

        return task.Unwrap();
    }

    public static Task<TResult> StartNewUnwrapped<TResult>(
        this TaskFactory factory,
        Func<object?, Task<TResult>> function,
        object? state)
    {
        var task = factory.StartNew(function, state);

        return task.Unwrap();
    }

    public static Task<TResult> StartNewUnwrapped<TResult>(
        this TaskFactory factory,
        Func<object?, Task<TResult>> function,
        object? state,
        CancellationToken cancellationToken)
    {
        var task = factory.StartNew(function, state, cancellationToken);

        return task.Unwrap();
    }

    public static Task<TResult> StartNewUnwrapped<TResult>(
        this TaskFactory factory,
        Func<object?, Task<TResult>> function,
        object? state,
        TaskCreationOptions creationOptions)
    {
        var task = factory.StartNew(function, state, creationOptions);

        return task.Unwrap();
    }

    public static Task<TResult> StartNewUnwrapped<TResult>(
        this TaskFactory factory,
        Func<Task<TResult>> function,
        TaskScheduler scheduler,
        CancellationToken? cancellationToken = null,
        TaskCreationOptions? creationOptions = null)
    {
        var task = factory.StartNew(
            function,
            cancellationToken ?? factory.CancellationToken,
            creationOptions ?? factory.CreationOptions,
            scheduler);

        return task.Unwrap();
    }

    public static Task<TResult> StartNewUnwrapped<TResult>(
        this TaskFactory factory,
        Func<object?, Task<TResult>> function,
        object? state,
        TaskScheduler scheduler,
        CancellationToken? cancellationToken = null,
        TaskCreationOptions? creationOptions = null)
    {
        var task = factory.StartNew(
            function,
            state,
            cancellationToken ?? factory.CancellationToken,
            creationOptions ?? factory.CreationOptions,
            scheduler);

        return task.Unwrap();
    }

    #endregion

}
