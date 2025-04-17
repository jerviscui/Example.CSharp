using Microsoft.AspNetCore.Mvc;

namespace CapTest.Depot.Host.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    #region Methods

    [HttpGet]
    [ProducesResponseType(typeof(string), 200)]
    public IActionResult Get()
    {
        return Ok("hello");
    }

    #endregion

}
