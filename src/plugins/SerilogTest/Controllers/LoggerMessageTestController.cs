using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace SerilogTest.Controllers;

[ApiController]
[Route("[controller]")]
public partial class LoggerMessageTestController : ControllerBase
{

    #region Constants & Statics

    private static void InnerMehtod(int? abc)
    {
        Console.WriteLine(abc);

        throw new MyException("the InnerMehtod throw");
    }

    #endregion

    private readonly ILogger<LoggerMessageTestController> _logger;

    public LoggerMessageTestController(ILogger<LoggerMessageTestController> logger)
    {
        _logger = logger;
    }

    #region Methods

    [HttpGet("DestructObject")]
    [ProducesResponseType<WeatherForecast>(((int)HttpStatusCode.OK))]
    public IActionResult DestructObject()
    {
        var dateTime = DateTime.Now.AddDays(1);
        var data = new WeatherForecast
        {
            Date = DateOnly.FromDateTime(dateTime),
            DateTime = dateTime,
            DateTimeOffset = DateTimeOffset.UtcNow,
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = "summary"
        };

        LogDestructObject(data);

        return Ok(data);
    }

    [HttpGet("LogErrorThrow")]
    public IActionResult LogErrorThrow(int? abc)
    {
        try
        {
            InnerMehtod(abc);
        }
        catch (Exception ex)
        {
            LogErrorMessage(ex);
        }

        return Ok();
    }

    [HttpGet("LoggerScope")]
    public IActionResult LoggerScope()
    {
        using (_logger.BeginScope("scope 1"))
        {
            // ["scope 1"]
            LogScopeMessage("sth. 1");

            using (_logger.BeginScope("scope 2"))
            {
                // ["scope 1", "scope 2"]
                LogScopeMessage("sth. 2");
            }
        }

        return Ok();
    }

    #endregion

    #region Logger

    [NonAction]
    [LoggerMessage(100, LogLevel.Information, "{Message}")]
    public partial void LogScopeMessage(string message);

    [NonAction]
    [LoggerMessage(200, LogLevel.Error, "there is a error.")]
    public partial void LogErrorMessage(Exception ex);

    [NonAction]
    [LoggerMessage(300, LogLevel.Information, "{@Data}")]
    public partial void LogDestructObject(WeatherForecast data);

    #endregion

}
