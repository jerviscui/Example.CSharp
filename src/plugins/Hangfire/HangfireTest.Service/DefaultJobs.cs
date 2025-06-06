using Hangfire;
using Microsoft.Extensions.Logging;

namespace HangfireTest.Service;

[Queue("default")]
[CustomServerFilter]
public class DefaultJobs
{
    private readonly ILogger<DefaultJobs> _logger;

    public DefaultJobs(ILogger<DefaultJobs> logger)
    {
        _logger = logger;
    }

    #region Methods

    [JobDisplayName("Job1, id is:{0}")]
    [AutomaticRetry(Attempts = 3)]
    public async Task Job1(int id)
    {
        await Task.Delay(500);
        //todo: how to get Processing Server and Worker
        _logger.LogInformation($"{nameof(DefaultJobs)}.{nameof(Job1)} completed.");
    }

    public async Task Job2(int seconds)
    {
        await Task.Delay(500);
        _logger.LogInformation($"{nameof(DefaultJobs)}.{nameof(Job2)} delay {seconds} seconds completed.");
    }

    [DisableConcurrentExecution(3)]
    public async Task Job3()
    {
        //并发时执行失败，自动重试

        var count = 0;
        while (count++ < 12)
        {
            await Task.Delay(6000);
            _logger.LogInformation($"{nameof(DefaultJobs)}.{nameof(Job3)} completed.");
        }
    }

    [SkipConcurrentExecution(3)]
    public async Task Job4()
    {
        //并发时标记为成功，continuation 被执行
        var count = 0;
        while (count++ < 12)
        {
            await Task.Delay(6000);
            _logger.LogInformation($"{nameof(DefaultJobs)}.{nameof(Job4)} completed.");
        }
    }

    public async Task Job5(CancellationToken cancellation)
    {
        //Cancell exception will be re-queued
        var count = 0;
        while (count++ < 12)
        {
            await Task.Delay(6000, cancellation);
            _logger.LogInformation($"{nameof(DefaultJobs)}.{nameof(Job5)} completed.");
        }
    }

    #endregion

}
