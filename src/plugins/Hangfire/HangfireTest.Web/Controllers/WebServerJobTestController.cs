using Hangfire;
using HangfireTest.Service;
using Microsoft.AspNetCore.Mvc;

namespace HangfireTest.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class WebServerJobTestController : ControllerBase
{
    [HttpGet("AddConsoleJob")]
    public IActionResult AddConsoleJob()
    {
        var id = BackgroundJob.Enqueue<WebServerJobs>(jobs => jobs.TestJob());

        return Ok();
    }
}
