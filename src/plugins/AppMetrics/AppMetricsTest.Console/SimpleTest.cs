using System.Text;
using App.Metrics;
using App.Metrics.Counter;
using App.Metrics.Scheduling;

namespace AppMetricsTest.Console;

internal static class SimpleTest
{
    public static async Task Test()
    {
        var builder = new MetricsBuilder();
        var metricsRoot = builder.Build();

        metricsRoot.Measure.Counter.Increment(new CounterOptions { Name = "SimpleTest" },
            new MetricTags("key1", "value1"), 2);

        var source = metricsRoot.Snapshot.Get();

        var stream = new MemoryStream();
        await metricsRoot.DefaultOutputMetricsFormatter.WriteAsync(stream, source);
        var content = Encoding.UTF8.GetString(stream.ToArray());

        System.Console.WriteLine(content);
    }

    public static AppMetricsTaskScheduler ConsoleReportTest()
    {
        var builder = new MetricsBuilder().Report.ToConsole();
        var metricsRoot = builder.Build();

#pragma warning disable CS4014
        Task.Run(async () =>
#pragma warning restore CS4014
        {
            int count = 10;
            while (count-- > 0)
            {
                metricsRoot.Measure.Counter.Increment(new CounterOptions { Name = "SimpleTest" },
                    new MetricTags("method", "ConsoleReportTest"), 2);

                await Task.Delay(1000);
            }
        });

        var scheduler = new AppMetricsTaskScheduler(TimeSpan.FromSeconds(2), async () =>
        {
            await Task.WhenAll(metricsRoot.ReportRunner.RunAllAsync());
        });
        scheduler.Start();

        return scheduler;
    }

    public static async Task ConfigurationTest()
    {
        var builder = new MetricsBuilder().Configuration
            .Configure(options =>
            {
                options.DefaultContextLabel = "MyContext";
                options.GlobalTags.Add("myTagKey", "myTagValue");
                options.Enabled = true;
                options.ReportingEnabled = true;
            });

        var metricsRoot = builder.Build();

        metricsRoot.Measure.Counter.Increment(new CounterOptions { Name = "ConfigurationTest" },
            new MetricTags("key1", "value1"), 2);

        var source = metricsRoot.Snapshot.Get();

        var stream = new MemoryStream();
        await metricsRoot.DefaultOutputMetricsFormatter.WriteAsync(stream, source);
        var content = Encoding.UTF8.GetString(stream.ToArray());

        System.Console.WriteLine(content);
    }

    public static async Task DefaultBuilderTest()
    {
        var builder = AppMetrics.CreateDefaultBuilder();

        var metricsRoot = builder.Build();

        metricsRoot.Measure.Counter.Increment(new CounterOptions { Name = "DefaultBuilderTest" },
            new MetricTags("key1", "value1"), 2);

        var source = metricsRoot.Snapshot.Get();

        var stream = new MemoryStream();
        await metricsRoot.DefaultOutputMetricsFormatter.WriteAsync(stream, source);
        var content = Encoding.UTF8.GetString(stream.ToArray());

        System.Console.WriteLine(content);
    }

    public static async Task ContextNameTest()
    {
        var builder = new MetricsBuilder().Configuration
            .Configure(options =>
            {
                options.DefaultContextLabel = "DefaultLabel";
            });

        var metricsRoot = builder.Build();

        metricsRoot.Measure.Counter.Increment(
            new CounterOptions
            {
                Name = "ConfigurationTest", Context = "ContextName"
            }, //Context 会覆盖 DefaultContextLabel 和 Name
            new MetricTags("key1", "value1"), 2);

        var source = metricsRoot.Snapshot.Get();

        var stream = new MemoryStream();
        await metricsRoot.DefaultOutputMetricsFormatter.WriteAsync(stream, source);
        var content = Encoding.UTF8.GetString(stream.ToArray());

        System.Console.WriteLine(content);

        //# TIMESTAMP: 637856135216510386
        //# MEASUREMENT: [ContextName] ConfigurationTest
        //# TAGS:
        //        key1 = value1
        //        mtype = counter
        //        unit = none
        //# FIELDS:
        //        value = 2
        //--------------------------------------------------------------
    }
}
