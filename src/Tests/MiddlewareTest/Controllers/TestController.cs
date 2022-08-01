using Microsoft.AspNetCore.Mvc;

namespace MiddlewareTest.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("1");
    }
}
