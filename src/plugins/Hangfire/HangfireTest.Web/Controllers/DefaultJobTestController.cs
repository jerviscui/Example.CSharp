using Hangfire;
using HangfireTest.Service;
using Microsoft.AspNetCore.Mvc;

namespace HangfireTest.Web.Controllers;

[ApiController]
[Route("[controller]")]
public class DefaultJobTestController : ControllerBase
{
    [HttpGet("AddDefaultJob")]
    public IActionResult AddDefaultJob()
    {
        var id = BackgroundJob.Enqueue<DefaultJobs>(jobs => jobs.Job1(1));

        return Ok();
    }

    [HttpGet("AddDelayJob")]
    public IActionResult AddDelayJob()
    {
        var id = BackgroundJob.Schedule<DefaultJobs>(jobs => jobs.Job2(2), TimeSpan.FromSeconds(2));

        return Ok();
    }

    [HttpGet("AddContinueJob")]
    public IActionResult AddContinueJob()
    {
        var id = BackgroundJob.Schedule<DefaultJobs>(jobs => jobs.Job2(2), TimeSpan.FromSeconds(2));
        BackgroundJob.ContinueJobWith<DefaultJobs>(id, jobs => jobs.Job2(1));

        return Ok();
    }

    [HttpGet("AddRecurringJob")]
    public IActionResult AddRecurringJob()
    {
        RecurringJob.AddOrUpdate<DefaultJobs>("RecurringJob1", jobs => jobs.Job3(), Cron.Minutely);

        return Ok();
    }

    [HttpGet("DisableConcurrentExecutionContinueTest")]
    public IActionResult DisableConcurrentExecutionContinueTest()
    {
        var id = BackgroundJob.Enqueue<DefaultJobs>(jobs => jobs.Job3());
        BackgroundJob.ContinueJobWith<DefaultJobs>(id, jobs => jobs.Job2(1));

        var id2 = BackgroundJob.Enqueue<DefaultJobs>(jobs => jobs.Job3());
        BackgroundJob.ContinueJobWith<DefaultJobs>(id2, jobs => jobs.Job2(1));

        return Ok();
    }

    [HttpGet("SkipConcurrentExecutionContinueTest")]
    public IActionResult SkipConcurrentExecutionContinueTest()
    {
        var id = BackgroundJob.Enqueue<DefaultJobs>(jobs => jobs.Job4());
        BackgroundJob.ContinueJobWith<DefaultJobs>(id, jobs => jobs.Job2(1));

        var id2 = BackgroundJob.Enqueue<DefaultJobs>(jobs => jobs.Job4());
        BackgroundJob.ContinueJobWith<DefaultJobs>(id2, jobs => jobs.Job2(1));

        return Ok();
    }

    [HttpGet("CancellationTokenTest")]
    public IActionResult CancellationTokenTest()
    {
        var id = BackgroundJob.Enqueue<DefaultJobs>(jobs => jobs.Job5(CancellationToken.None));
        BackgroundJob.ContinueJobWith<DefaultJobs>(id, jobs => jobs.Job2(1));

        return Ok();
    }

    [HttpGet("TriggerRecurringJobTest")]
    public IActionResult TriggerRecurringJobTest()
    {
        RecurringJob.AddOrUpdate<DefaultJobs>("RecurringJob2", jobs => jobs.Job4(), Cron.Minutely);
        //Trigger 执行 JobFilterAttribute 也会生效
        RecurringJob.Trigger("RecurringJob2");

        return Ok();
    }
}
