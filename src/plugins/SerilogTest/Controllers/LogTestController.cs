using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using Serilog.Context;
using System.Net;

namespace SerilogTest.Controllers;

[ApiController]
[Route("[controller]")]
public class LogTestController : ControllerBase
{

    #region Constants & Statics

    private static void InnerMehtod(int? abc)
    {
        Console.WriteLine(abc);

        throw new MyException("the InnerMehtod throw");
    }

    #endregion

    private readonly string[] _summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    private readonly IHttpContextAccessor _httpContextAccessor;

    private readonly ILogger<LogTestController> _logger;

    public LogTestController(ILogger<LogTestController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    #region Methods

    [HttpGet("LogError")]
    public IActionResult LogError()
    {
        _logger.LogError(new MyException("LogError action called"), "this is error message.");

        return Ok();
    }

    [HttpGet("LogErrorThrow")]
    public IActionResult LogErrorThrow(int? abc)
    {
        InnerMehtod(abc);

        return Ok();
    }

    [HttpGet("LoggerScope")]
    [HttpLogging(HttpLoggingFields.RequestPropertiesAndHeaders)]
    [ProducesResponseType<WeatherForecast>(((int)HttpStatusCode.OK))]
    public IActionResult LoggerScope()
    {
        using (_logger.BeginScope("scope 1"))
        {
            // ["scope 1"]
            _logger.LogInformation("sth. 1");

            using (_logger.BeginScope("scope 2"))
            {
                // ["scope 1", "scope 2"]
                _logger.LogInformation("sth. 2");
            }
        }

        return Ok();
    }

    [HttpGet("LogWarning")]
    public IEnumerable<WeatherForecast> LogWarning()
    {
        _logger.LogInformation("LogWarning calling");

        var arr = Enumerable.Range(1, 5)
            .Select(
                index =>
                {
                    var dateTime = DateTime.Now.AddDays(index);
                    return new WeatherForecast
                    {
                        Date = DateOnly.FromDateTime(dateTime),
                        DateTime = dateTime,
                        DateTimeOffset = DateTimeOffset.UtcNow,
                        TemperatureC = Random.Shared.Next(-20, 55),
                        Summary = _summaries[Random.Shared.Next(_summaries.Length)]
                    };
                })
            .ToArray();

        _logger.LogWarning("result is {@Arr}", arr);

        return arr;
    }

    [HttpGet("OnlyLogRequest")]
    [HttpLogging(HttpLoggingFields.RequestPropertiesAndHeaders)]
    [ProducesResponseType<WeatherForecast>(((int)HttpStatusCode.OK))]
    public IActionResult OnlyLogRequest()
    {
        var dateTime = DateTime.Now.AddDays(1);

        return Ok(
            new WeatherForecast
            {
                Date = DateOnly.FromDateTime(dateTime),
                DateTime = dateTime,
                DateTimeOffset = DateTimeOffset.UtcNow,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries[Random.Shared.Next(_summaries.Length)]
            });
    }

    [HttpGet("UseLogContext")]
    [ProducesResponseType<WeatherForecast>(((int)HttpStatusCode.OK))]
    public IActionResult UseLogContext()
    {
        using var _ = LogContext.PushProperty("TenantId", Guid.NewGuid());
        using var __ = LogContext.PushProperty("UserId", 1);

        _logger.LogInformation("method starting: {Method}", nameof(UseLogContext));

        var dateTime = DateTime.Now.AddDays(1);

        return Ok(
            new WeatherForecast
            {
                Date = DateOnly.FromDateTime(dateTime),
                DateTime = dateTime,
                DateTimeOffset = DateTimeOffset.UtcNow,
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = _summaries[Random.Shared.Next(_summaries.Length)]
            });
    }

    #endregion

}
