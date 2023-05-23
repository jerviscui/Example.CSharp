using Microsoft.Extensions.Logging;

namespace LoggerMessageTest;

public static partial class LoggerMessageAttributeExtensions
{
    [LoggerMessage(
        EventId = 1002,
        Level = LogLevel.Error,
        Message = "Failed to connect to the database"
    )]
    public static partial void DatabaseConnectionFailed(this ILogger logger, Exception ex);

    [LoggerMessage(
        EventId = 1003,
        Level = LogLevel.Error,
        Message = "Failed to connect to the database"
    )]
    public static partial void FailedWithoutThis(ILogger logger, Exception ex);

    [LoggerMessage(
        EventId = 1004,
        Level = LogLevel.Information,
        Message = "Welcome to {city} {province}!"
    )]
    public static partial void CityInfo(this ILogger logger, string city, string province);

    [LoggerMessage(
        EventId = 1005,
        Message = "Welcome to {city} {province}!")]
    public static partial void
        LogWithDynamicLogLevel(this ILogger logger, LogLevel level, string city, string province);
}
