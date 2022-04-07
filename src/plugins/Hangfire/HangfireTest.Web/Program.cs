using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Redis;
using HangfireTest.Service;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddTestJobs();

services.AddHangfire(configuration =>
{
    configuration.UseRedisStorage("10.99.59.47:7000,DefaultDatabase=7,allowAdmin=true", new RedisStorageOptions());

    foreach (var metric in DashboardMetrics.GetMetrics())
    {
        configuration.UseDashboardMetric(metric);
    }

    configuration.UseDashboardMetric(
        RedisStorage.GetDashboardMetricFromRedisInfo("使用内存", RedisInfoKeys.used_memory_human));
    configuration.UseDashboardMetric(
        RedisStorage.GetDashboardMetricFromRedisInfo("高峰内存", RedisInfoKeys.used_memory_peak_human));
});

var jobServerOptions = new BackgroundJobServerOptions { ServerName = "HangfireTest.Web", Queues = new[] { "default" } };
services.AddSingleton(jobServerOptions);
services.AddHangfireServer(options =>
{
    options.ServerName = jobServerOptions.ServerName;
    options.Queues = jobServerOptions.Queues;
});

var app = builder.Build();

var options = new DashboardOptions
{
    Authorization = new IDashboardAuthorizationFilter[] { new AllowAnonymousAuthorizationFilter() },
    IgnoreAntiforgeryToken = true
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
