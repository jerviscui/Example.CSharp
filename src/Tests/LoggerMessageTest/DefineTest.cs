using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace LoggerMessageTest;

public class DefineTest
{
    private readonly ILogger<DefineTest> _logger;

    public DefineTest()
    {
        var loggerFactory = new LoggerFactory(new[]
        {
            new ConsoleLoggerProvider(
                new OptionsMonitor<ConsoleLoggerOptions>(
                    new OptionsFactory<ConsoleLoggerOptions>(
                        new List<IConfigureOptions<ConsoleLoggerOptions>>(),
                        new List<IPostConfigureOptions<ConsoleLoggerOptions>>()),
                    Array.Empty<IOptionsChangeTokenSource<ConsoleLoggerOptions>>(),
                    new OptionsCache<ConsoleLoggerOptions>()))
        });

        _logger = loggerFactory.CreateLogger<DefineTest>();
    }

    private static readonly Action<ILogger, string, int, Exception?> ErrorMessage =
        LoggerMessage.Define<string, int>(LogLevel.Error, new EventId(0, "ERROR"), "{Message} {Count}");

    public void LogTest()
    {
        ErrorMessage(_logger, "sth.", 123, null);
    }

    public void LogExtensionTest()
    {
        _logger.InfoMessage("sth.", 1);
    }
}

public static class LoggerExtensions
{
    private static readonly Action<ILogger, string, int, Exception?> TestInfoMessage =
        LoggerMessage.Define<string, int>(LogLevel.Information, new EventId(0, "ERROR"), "{Message} {Count}");

    public static void InfoMessage(this ILogger logger, string message, int count)
    {
        TestInfoMessage(logger, message, count, null);
    }
}
