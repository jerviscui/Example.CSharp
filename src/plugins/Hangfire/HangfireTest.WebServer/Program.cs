using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Redis;
using Hangfire.Server;
using HangfireTest.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddTestJobs();

services.TryAddSingleton<IBackgroundJobPerformer>(x => new CustomBackgroundJobPerformer(
    new BackgroundJobPerformer(x.GetRequiredService<IJobFilterProvider>(), x.GetRequiredService<JobActivator>(),
        TaskScheduler.Default)));

services.AddHangfire(configuration =>
{
    configuration.UseRedisStorage("10.99.59.47:7000,DefaultDatabase=7,allowAdmin=true");

    foreach (var metric in DashboardMetrics.GetMetrics())
    {
        configuration.UseDashboardMetric(metric);
    }

    configuration.UseDashboardMetric(
        RedisStorage.GetDashboardMetricFromRedisInfo("使用内存", RedisInfoKeys.used_memory_human));
    configuration.UseDashboardMetric(
        RedisStorage.GetDashboardMetricFromRedisInfo("高峰内存", RedisInfoKeys.used_memory_peak_human));
});

var jobServerOptions =
    new BackgroundJobServerOptions { ServerName = "HangfireTest.Web", Queues = new[] { "default", "webserver" } };
services.AddSingleton(jobServerOptions);
services.AddHangfireServer(options =>
{
    options.ServerName = jobServerOptions.ServerName;
    options.Queues = jobServerOptions.Queues;
    options.WorkerCount = 1;
});

var app = builder.Build();

var options = new DashboardOptions
{
    Authorization = new[] { new AllowAnonymousAuthorizationFilter() }, IgnoreAntiforgeryToken = true
};
if (app.Environment.IsProduction())
{
    options.DisplayStorageConnectionString = false;
}
app.UseHangfireDashboard("/hangfire", options);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

internal sealed class CustomBackgroundJobPerformer : IBackgroundJobPerformer
{
    private readonly IBackgroundJobPerformer _inner;

    public CustomBackgroundJobPerformer(IBackgroundJobPerformer inner)
    {
        _inner = inner ?? throw new ArgumentNullException(nameof(inner));
    }

    public object Perform(PerformContext context)
    {
        Console.WriteLine(
            $"Perform {context.BackgroundJob.Id} ({context.BackgroundJob.Job.Type.FullName}.{context.BackgroundJob.Job.Method.Name})");
        return _inner.Perform(context);
    }
}
