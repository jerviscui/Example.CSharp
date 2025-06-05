using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Redis.StackExchange;

namespace HangfireTest.Server;

internal static class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        var configuration = GlobalConfiguration.Configuration;
        _ = configuration.UseRedisStorage("127.0.0.1:6379,DefaultDatabase=7,allowAdmin=true");
        _ = configuration.UseColouredConsoleLogProvider();
        //todo: add a mini web dashboard?
        foreach (var metric in DashboardMetrics.GetMetrics())
        {
            _ = configuration.UseDashboardMetric(metric);
        }
        _ = configuration.UseDashboardMetric(
            RedisStorage.GetDashboardMetricFromRedisInfo("使用内存", RedisInfoKeys.used_memory_human));
        _ = configuration.UseDashboardMetric(
            RedisStorage.GetDashboardMetricFromRedisInfo("高峰内存", RedisInfoKeys.used_memory_peak_human));

        _ = BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

        var jobServerOptions =
            new BackgroundJobServerOptions { ServerName = "HangfireTest.Web", Queues = new[] { "default", "console" } };
        using var server = new BackgroundJobServer(jobServerOptions);

        Console.WriteLine("Hangfire Server started. Press any key to exit...");
        _ = Console.ReadKey();
    }

    #endregion

}
