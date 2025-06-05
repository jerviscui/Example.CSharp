using Hangfire;
using Microsoft.Extensions.Logging;

namespace HangfireTest.Service;

[Queue("console")]
public class ConsoleJobs
{
    private readonly ILogger<ConsoleJobs> _logger;

    public ConsoleJobs(ILogger<ConsoleJobs> logger)
    {
        _logger = logger;
    }

    #region Methods

    public async Task TestJob()
    {
        await Task.Delay(500);
        _logger.LogInformation($"console job:{nameof(TestJob)} completed.");
    }

    #endregion

}
