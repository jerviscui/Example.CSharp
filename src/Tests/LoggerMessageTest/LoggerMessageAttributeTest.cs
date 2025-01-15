using Microsoft.Extensions.Logging;

namespace LoggerMessageTest;

public class LoggerMessageAttributeTest
{
    private readonly ILogger<LoggerMessageAttributeTest> _logger;

    public LoggerMessageAttributeTest(ILogger<LoggerMessageAttributeTest> logger) => _logger = logger;

    public void LogErrorTest()
    {
        try
        {
#pragma warning disable CA2201 // Do not raise reserved exception types
            throw new Exception("test test.");
#pragma warning restore CA2201 // Do not raise reserved exception types
        }
        catch (Exception e)
        {
            _logger.DatabaseConnectionFailed(e);
        }
    }

    public void LogError_WithoutThis_Test()
    {
#pragma warning disable CA2201 // Do not raise reserved exception types
        LoggerMessageAttributeExtensions.FailedWithoutThis(_logger, new Exception("test WithoutThis"));
#pragma warning restore CA2201 // Do not raise reserved exception types
    }

    public void LogInfo_WithParameter_Test()
    {
        _logger.CityInfo("sz", "gz");
    }

    public void DynamicLevel_Info_Test()
    {
        _logger.LogWithDynamicLogLevel(LogLevel.Information, "info", "gz");
    }

    public void DynamicLevel_Error_Test()
    {
        _logger.LogWithDynamicLogLevel(LogLevel.Error, "error", "gz");
    }
}
