using Microsoft.Extensions.Logging;

namespace LoggerMessageTest;

public partial class LoggingSample
{

    #region Constants & Statics

    [LoggerMessage(EventId = 20, Level = LogLevel.Critical, Message = "Value is {Value:E}")]
    public static partial void UsingFormatSpecifier(ILogger logger, double value);

    #endregion

    private readonly ILogger _logger;

    public LoggingSample(ILogger logger)
    {
        _logger = logger;
    }

    #region Methods

    [LoggerMessage(EventId = 9, Level = LogLevel.Trace, Message = "Fixed message", EventName = "CustomEventName")]
#pragma warning disable CA1822 // Mark members as static
    public partial void LogWithCustomEventName();
#pragma warning restore CA1822 // Mark members as static

    [LoggerMessage(EventId = 10, Message = "Welcome to {City} {Province}!")]
#pragma warning disable CA1822 // Mark members as static
    public partial void LogWithDynamicLogLevel(string city, LogLevel level, string province);
#pragma warning restore CA1822 // Mark members as static

#pragma warning disable CA1822 // Mark members as static
    public void TestLogging()
#pragma warning restore CA1822 // Mark members as static
    {
        LogWithCustomEventName();

        LogWithDynamicLogLevel("Vancouver", LogLevel.Warning, "BC");
        LogWithDynamicLogLevel("Vancouver", LogLevel.Information, "BC");

        UsingFormatSpecifier(_logger, 12345.6789);
    }

    #endregion

}
