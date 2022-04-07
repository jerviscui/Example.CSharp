using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace HangfireTest.Service;

[Queue("default")]
public class DefaultJobs
{
    private readonly ILogger<DefaultJobs> _logger;

    private readonly BackgroundJobServerOptions _options;

    public DefaultJobs(ILogger<DefaultJobs> logger, BackgroundJobServerOptions options)
    {
        _logger = logger;
        _options = options;
    }

    [JobDisplayName("Job1, id is:{0}")]
    [AutomaticRetry(Attempts = 3)]
    public async Task Job1(int id)
    {
        await Task.Delay(500);
        //todo: how to get Processing Server and Worker
        //_logger.LogInformation($"{nameof(DefaultJobs)}.{nameof(Job1)} @{_options.ServerName} completed.");
        _logger.LogInformation($"{nameof(DefaultJobs)}.{nameof(Job1)} completed.");
    }

    public async Task Job2(int seconds)
    {
        await Task.Delay(500);
        _logger.LogInformation(
            $"{nameof(DefaultJobs)}.{nameof(Job2)} delay {seconds} seconds completed.");
    }

    [DisableConcurrentExecution(3)]
    public async Task Job3()
    {
        //并发时执行失败，自动重试

        int count = 0;
        while (count++ < 12)
        {
            await Task.Delay(6000);
            _logger.LogInformation(
                $"{nameof(DefaultJobs)}.{nameof(Job3)} completed.");
        }
    }

    [SkipConcurrentExecution(3)]
    public async Task Job4()
    {
        //并发时标记为成功，continuation 被执行
        int count = 0;
        while (count++ < 12)
        {
            await Task.Delay(6000);
            _logger.LogInformation(
                $"{nameof(DefaultJobs)}.{nameof(Job4)} completed.");
        }
    }

    public async Task Job5(CancellationToken cancellation)
    {
        //Cancell exception will be re-queued
        int count = 0;
        while (count++ < 12)
        {
            await Task.Delay(6000, cancellation);
            _logger.LogInformation(
                $"{nameof(DefaultJobs)}.{nameof(Job5)} completed.");
        }
    }
}
