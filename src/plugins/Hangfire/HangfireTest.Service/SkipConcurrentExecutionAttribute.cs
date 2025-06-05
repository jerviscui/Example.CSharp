using Hangfire.Common;
using Hangfire.Server;
using Hangfire.Storage;

namespace HangfireTest.Service;

/// <summary>
/// 忽略并发执行的任务
/// </summary>
public class SkipConcurrentExecutionAttribute : JobFilterAttribute, IServerFilter
{

    #region Constants & Statics

    private static string GetResource(Job job)
    {
        return job.ToString();
    }

    #endregion

    private readonly int _timeoutInSeconds;

    /// <summary>
    /// Initializes a new instance of the <see cref="SkipConcurrentExecutionAttribute"/> class.
    /// </summary>
    /// <param name="timeoutInSeconds">The timeout in seconds.</param>
    /// <exception cref="ArgumentException">Timeout argument value should be greater that zero.</exception>
    public SkipConcurrentExecutionAttribute(int timeoutInSeconds)
    {
        _timeoutInSeconds = timeoutInSeconds >= 0
            ? timeoutInSeconds
            : throw new ArgumentException("Timeout argument value should be greater that zero.");
    }

    #region IServerFilter implementations

    /// <inheritdoc/>
    public void OnPerformed(PerformedContext filterContext)
    {
        if (!filterContext.Items.TryGetValue("DistributedLock", out var value))
        {
            throw new InvalidOperationException("Can not release a distributed lock: it was not acquired.");
        }
        ((IDisposable)value).Dispose();
    }

    /// <inheritdoc/>
    public void OnPerforming(PerformingContext filterContext)
    {
        var resource = GetResource(filterContext.BackgroundJob.Job);
        var timeout = TimeSpan.FromSeconds(_timeoutInSeconds);
        try
        {
            var disposable = filterContext.Connection.AcquireDistributedLock(resource, timeout);
            filterContext.Items["DistributedLock"] = disposable;
        }
        catch (DistributedLockTimeoutException)
        {
            filterContext.Canceled = true;
        }
    }

    #endregion

}
