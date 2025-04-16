using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace LoggerMessageTest;

public class DefineTest
{

    #region Constants & Statics

    private static readonly Action<ILogger, string, int, Exception?> ErrorMessage =
        LoggerMessage.Define<string, int>(LogLevel.Error, new EventId(0, "ERROR"), "{Message} {Count}");

    #endregion

    private readonly ILogger<DefineTest> _logger;

    public DefineTest()
    {
        using var optionsMonitor = new OptionsMonitor<ConsoleLoggerOptions>(
            new OptionsFactory<ConsoleLoggerOptions>([], []),
            [],
            new OptionsCache<ConsoleLoggerOptions>());
        using var loggerFactory = new LoggerFactory(
            new[]
            {
                new ConsoleLoggerProvider(
                optionsMonitor)
            });

        _logger = loggerFactory.CreateLogger<DefineTest>();
    }

    #region Methods

    public void LogExtensionTest()
    {
        _logger.InfoMessage("sth.", 1);
    }

    public void LogTest()
    {
        ErrorMessage(_logger, "sth.", 123, null);
    }

    #endregion

}

public static class LoggerExtensions
{

    #region Constants & Statics

    private static readonly Action<ILogger, string, int, Exception?> TestInfoMessage =
        LoggerMessage.Define<string, int>(LogLevel.Information, new EventId(0, "ERROR"), "{Message} {Count}");

    public static void InfoMessage(this ILogger logger, string message, int count)
    {
        TestInfoMessage(logger, message, count, null);
    }

    #endregion

}
