using System;
using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;

namespace HangfireTest.Service;

/// <summary>
/// 忽略并发执行的任务
/// </summary>
public class SkipConcurrentExecutionAttribute : JobFilterAttribute, IServerFilter
{
    private readonly int _timeoutInSeconds;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkipConcurrentExecutionAttribute"/> class.
    /// </summary>
    /// <param name="timeoutInSeconds">The timeout in seconds.</param>
    /// <exception cref="System.ArgumentException">Timeout argument value should be greater that zero.</exception>
    public SkipConcurrentExecutionAttribute(int timeoutInSeconds) => _timeoutInSeconds = timeoutInSeconds >= 0
        ? timeoutInSeconds
        : throw new ArgumentException("Timeout argument value should be greater that zero.");

    /// <inheritdoc />
    public void OnPerforming(PerformingContext filterContext)
    {
        string resource = GetResource(filterContext.BackgroundJob.Job);
        TimeSpan timeout = TimeSpan.FromSeconds(_timeoutInSeconds);
        try
        {
            IDisposable disposable = filterContext.Connection.AcquireDistributedLock(resource, timeout);
            filterContext.Items["DistributedLock"] = disposable;
        }
        catch (DistributedLockTimeoutException)
        {
            filterContext.Canceled = true;
        }
    }

    /// <inheritdoc />
    public void OnPerformed(PerformedContext filterContext)
    {
        if (!filterContext.Items.ContainsKey("DistributedLock"))
        {
            throw new InvalidOperationException("Can not release a distributed lock: it was not acquired.");
        }
        ((IDisposable)filterContext.Items["DistributedLock"]).Dispose();
    }

    private static string GetResource(Job job) => job.ToString();
}
