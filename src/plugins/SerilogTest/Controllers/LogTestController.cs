using Microsoft.AspNetCore.Mvc;
using Serilog.Context;

namespace SerilogTest.Controllers;

[ApiController]
[Route("[controller]")]
public class LogTestController : ControllerBase
{

    #region Constants & Statics

    private static readonly string[] Summaries = ["Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"];

    #endregion

    private readonly ILogger<LogTestController> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LogTestController(ILogger<LogTestController> logger, IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    #region Methods

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
                        Summary = Summaries[Random.Shared.Next(Summaries.Length)]
                    };
                })
            .ToArray();

        _logger.LogWarning("result is {@Arr}", arr);

        return arr;
    }

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

    private static void InnerMehtod(int? abc)
    {
        Console.WriteLine(abc);

        throw new MyException("the InnerMehtod throw");
    }

    [HttpGet("UseLogContext")]
    public IActionResult UseLogContext()
    {
        using var _ = LogContext.PushProperty("TenantId", Guid.NewGuid());
        using var __ = LogContext.PushProperty("UserId", 1);

        _logger.LogInformation("method starting: {Method}", nameof(UseLogContext));

        return Ok();
    }

    #endregion

}
