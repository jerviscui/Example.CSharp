using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;

namespace MinimalApiTest.WebApi;

/// <summary>
/// MyController
/// </summary>
[ApiController]
[Produces(MediaTypeNames.Application.Json)]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
    /// <summary>
    /// Gets the test data.
    /// </summary>
    /// <response code="200">Returns the specified item</response>
    [HttpGet]
    [ProducesResponseType(typeof(WeatherForecast), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetTest()
    {
        return Ok(await Task.FromResult(new WeatherForecast(DateTime.Now, 10, "get test")));
    }
}
