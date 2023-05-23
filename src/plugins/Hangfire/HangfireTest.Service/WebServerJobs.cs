using Hangfire;
using Microsoft.Extensions.Logging;

namespace HangfireTest.Service;

[Queue("webserver")]
public class WebServerJobs
{
    private readonly ILogger<DefaultJobs> _logger;

    private readonly BackgroundJobServerOptions _options;

    public WebServerJobs(ILogger<DefaultJobs> logger, BackgroundJobServerOptions options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task TestJob()
    {
        await Task.Delay(2000);
        _logger.LogInformation($"webserver job:{nameof(TestJob)} completed.");
    }
}
