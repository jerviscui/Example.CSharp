using App.Metrics;
using App.Metrics.Meter;
using App.Metrics.Scheduling;

namespace AppMetricsTest.Console;

internal sealed class MeterTest : IDisposable
{
    private static readonly MeterOptions CacheHitsMeter = new() { Name = "Cache Hits", MeasurementUnit = Unit.Calls };

    private static readonly MeterOptions HttpStatusMeter = new() { Name = "Http Status", MeasurementUnit = Unit.Calls };

    private readonly AppMetricsTaskScheduler _scheduler;

    private readonly IMetricsRoot _metrics;

    public MeterTest()
    {
        _metrics = AppMetrics.CreateDefaultBuilder()
            .Configuration.Configure(options =>
            {
            })
            .Report.ToConsole()
            .Build();

        _scheduler = new AppMetricsTaskScheduler(TimeSpan.FromSeconds(1), async () =>
        {
            await Task.WhenAll(_metrics.ReportRunner.RunAllAsync());
            System.Console.WriteLine("----------------");
        });
        _scheduler.Start();
    }

    public void CacheHitsMeter_Test()
    {
        _metrics.Measure.Meter.Mark(CacheHitsMeter, 1);
        Thread.Sleep(2000);
        _metrics.Measure.Meter.Mark(CacheHitsMeter, 10);

        //"meters": [
        //    {
        //        "count": 1,
        //        "fifteenMinuteRate": 0,
        //        "fiveMinuteRate": 0,
        //        "items": [],
        //        "meanRate": 55.13330590145068,
        //        "oneMinuteRate": 0,
        //        "rateUnit": "min",
        //        "name": "Cache Hits|server:DESKTOP-6IBBCA1,app:AppMetricsTest.Console,env:release",
        //        "tags": {
        //            "server": "DESKTOP-6IBBCA1",
        //            "app": "AppMetricsTest.Console",
        //            "env": "release"
        //        },
        //        "unit": "calls"
        //    }
        //],
    }

    public void HttpStatusMeter_Test()
    {
        _metrics.Measure.Meter.Mark(HttpStatusMeter, "200");
        _metrics.Measure.Meter.Mark(HttpStatusMeter, "404");
        _metrics.Measure.Meter.Mark(HttpStatusMeter, "301");

        //"meters": [
        //    {
        //        "count": 3,
        //        "fifteenMinuteRate": 0,
        //        "fiveMinuteRate": 0,
        //        "items": [
        //        {
        //            "count": 1,
        //            "fifteenMinuteRate": 0,
        //            "fiveMinuteRate": 0,
        //            "item": "200",
        //            "meanRate": 55.91860711223172,
        //            "oneMinuteRate": 0,
        //            "percent": 33.33333333333333
        //        },
        //        {
        //            "count": 1,
        //            "fifteenMinuteRate": 0,
        //            "fiveMinuteRate": 0,
        //            "item": "301",
        //            "meanRate": 55.91860711223172,
        //            "oneMinuteRate": 0,
        //            "percent": 33.33333333333333
        //        },
        //        {
        //            "count": 1,
        //            "fifteenMinuteRate": 0,
        //            "fiveMinuteRate": 0,
        //            "item": "404",
        //            "meanRate": 55.91860711223172,
        //            "oneMinuteRate": 0,
        //            "percent": 33.33333333333333
        //        }
        //        ],
        //        "meanRate": 167.75582133669513,
        //        "oneMinuteRate": 0,
        //        "rateUnit": "min",
        //        "name": "Http Status|server:DESKTOP-6IBBCA1,app:AppMetricsTest.Console,env:release",
        //        "tags": {
        //            "server": "DESKTOP-6IBBCA1",
        //            "app": "AppMetricsTest.Console",
        //            "env": "release"
        //        },
        //        "unit": "calls"
        //    }
        //],
    }

    public void Reset_Test()
    {
        _metrics.Provider.Meter.Instance(HttpStatusMeter).Reset();
    }

    /// <inheritdoc />
    public void Dispose() => _scheduler.Dispose();
}
