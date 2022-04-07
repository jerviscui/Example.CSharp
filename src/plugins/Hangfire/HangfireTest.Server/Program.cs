using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Redis;

var configuration = GlobalConfiguration.Configuration;
configuration.UseRedisStorage("10.99.59.47:7000,DefaultDatabase=7,allowAdmin=true");
configuration.UseColouredConsoleLogProvider();
//todo: add a mini web dashboard?
foreach (var metric in DashboardMetrics.GetMetrics())
{
    configuration.UseDashboardMetric(metric);
}
configuration.UseDashboardMetric(
    RedisStorage.GetDashboardMetricFromRedisInfo("使用内存", RedisInfoKeys.used_memory_human));
configuration.UseDashboardMetric(
    RedisStorage.GetDashboardMetricFromRedisInfo("高峰内存", RedisInfoKeys.used_memory_peak_human));

BackgroundJob.Enqueue(() => Console.WriteLine("Hello, world!"));

var jobServerOptions =
    new BackgroundJobServerOptions { ServerName = "HangfireTest.Web", Queues = new[] { "default", "console" } };
using (var server = new BackgroundJobServer(jobServerOptions))
{
    Console.WriteLine("Hangfire Server started. Press any key to exit...");
    Console.ReadKey();
}
