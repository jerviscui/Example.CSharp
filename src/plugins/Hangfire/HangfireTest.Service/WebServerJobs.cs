using Hangfire;
using Microsoft.Extensions.Logging;

namespace HangfireTest.Service;

[Queue("webserver")]
public class WebServerJobs
{
    private readonly ILogger<WebServerJobs> _logger;

    public WebServerJobs(ILogger<WebServerJobs> logger)
    {
        _logger = logger;
    }

    #region Methods

    public async Task TestJob()
    {
        await Task.Delay(2000);
        _logger.LogInformation($"webserver job:{nameof(TestJob)} completed.");
    }

    #endregion

}
