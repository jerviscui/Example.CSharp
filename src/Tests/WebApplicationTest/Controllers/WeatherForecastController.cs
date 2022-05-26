using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace WebApplicationTest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("GetOptions")]
        public IActionResult GetOptions([FromServices] IOptionsSnapshot<SimpleOptions> simpleOptions,
            [FromServices] IOptionsSnapshot<ReadOnlyOptions> readonlyOptions,
            [FromServices] IOptionsSnapshot<PrivateOptions> privateOptions)
        {
            var obj = new JsonObject(new KeyValuePair<string, JsonNode?>[]
            {
                new("simpleOptions", JsonNode.Parse(JsonSerializer.Serialize(simpleOptions.Value))),
                new("readonlyOptions", JsonNode.Parse(JsonSerializer.Serialize(readonlyOptions.Value))),
                new("privateOptions", JsonNode.Parse(JsonSerializer.Serialize(privateOptions.Value)))
            });

            return Ok(obj);
        }
    }
}
