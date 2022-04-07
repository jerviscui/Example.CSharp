using Hangfire;
using HangfireTest.Service;
using Microsoft.AspNetCore.Mvc;

namespace HangfireTest.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class ConsoleJobTestController : ControllerBase
{
    [HttpGet("AddConsoleJob")]
    public IActionResult AddConsoleJob()
    {
        var id = BackgroundJob.Enqueue<ConsoleJobs>(jobs => jobs.TestJob());

        return Ok();
    }
}
