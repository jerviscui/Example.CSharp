using Hangfire;
using Hangfire.Common;
using Hangfire.Dashboard;
using Hangfire.Redis.StackExchange;
using Hangfire.Server;
using HangfireTest.Service;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace HangfireTest.WebServer;

internal static class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var services = builder.Services;
        _ = services.AddControllers();
        _ = services.AddEndpointsApiExplorer();

        _ = services.AddTestJobs();
        _ = services.AddSingleton<ExternalDataFilterAttribute>();

        services.TryAddSingleton<IBackgroundJobPerformer>(
            x =>
                new CustomBackgroundJobPerformer(
                new BackgroundJobPerformer(
                    x.GetRequiredService<IJobFilterProvider>(),
                    x.GetRequiredService<JobActivator>(),
                    TaskScheduler.Default)));

        _ = services.AddHangfire(
            (serviceProvider, configuration) =>
            {
                _ = configuration.UseRedisStorage("127.0.0.1:6379,DefaultDatabase=7,allowAdmin=true");

                var filter = serviceProvider.GetRequiredService<ExternalDataFilterAttribute>();
                _ = configuration.UseFilter(filter);

                foreach (var metric in DashboardMetrics.GetMetrics())
                {
                    _ = configuration.UseDashboardMetric(metric);
                }

                //_ = configuration.UseDashboardMetric(
                //    RedisStorage.GetDashboardMetricFromRedisInfo("使用内存", RedisInfoKeys.used_memory_human));
                //_ = configuration.UseDashboardMetric(
                //    RedisStorage.GetDashboardMetricFromRedisInfo("高峰内存", RedisInfoKeys.used_memory_peak_human));
            });

        _ = services.AddHangfireServer(
            options =>
            {
                options.ServerName = "HangfireTest.Web";
                options.Queues = ["default", "webserver"];
                options.WorkerCount = 1;
            });

        var app = builder.Build();

        var options = new DashboardOptions
        {
            Authorization = [new AllowAnonymousAuthorizationFilter()],
            IgnoreAntiforgeryToken = true,
            StatsPollingInterval = 5000
        };
        if (app.Environment.IsProduction())
        {
            options.DisplayStorageConnectionString = false;
        }
        _ = app.UseHangfireDashboard("/hangfire", options);

        // Configure the HTTP request pipeline.
        _ = app.UseHttpsRedirection();

        _ = app.UseAuthorization();

        _ = app.MapControllers();

        app.Run();
    }

    #endregion

}
