using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace WebApplicationTest.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{

    #region Methods

    [HttpGet]
    [Route("GetOptions")]
    public IActionResult GetOptions(
        [FromServices] IOptionsSnapshot<SimpleOptions> simpleOptions,
        [FromServices] IOptionsSnapshot<ReadOnlyOptions> readonlyOptions,
        [FromServices] IOptionsSnapshot<PrivateOptions> privateOptions)
    {
        var obj = new JsonObject(
            [
                new("simpleOptions", JsonNode.Parse(JsonSerializer.Serialize(simpleOptions.Value))),
                new("readonlyOptions", JsonNode.Parse(JsonSerializer.Serialize(readonlyOptions.Value))),
                new("privateOptions", JsonNode.Parse(JsonSerializer.Serialize(privateOptions.Value)))
            ]);

        return Ok(obj);

        //{
        //  "simpleOptions": {
        //    "InitArray": [
        //      "123"
        //    ],
        //    "InitArray2": [
        //      "123"
        //    ],
        //    "InitArray3": [
        //      "123"
        //    ],
        //    "Parkings": [
        //      "2323"
        //    ]
        //  },

        //  "readonlyOptions": {
        //    "Codes": null,
        //    "Parkings": [
        //      "123"
        //    ]
        //  },

        //  "privateOptions": {
        //    "Parkings": [
        //      "456"
        //    ]
        //  }
        //}
    }

    #endregion

}
