using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace HangfireTest.Service;

[Queue("console")]
public class ConsoleJobs
{
    private readonly ILogger<DefaultJobs> _logger;

    private readonly BackgroundJobServerOptions _options;

    public ConsoleJobs(ILogger<DefaultJobs> logger, BackgroundJobServerOptions options)
    {
        _logger = logger;
        _options = options;
    }

    public async Task TestJob()
    {
        await Task.Delay(500);
        _logger.LogInformation($"console job:{nameof(TestJob)} completed.");
    }
}
