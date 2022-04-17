using App.Metrics;
using App.Metrics.Scheduling;
using App.Metrics.Timer;

namespace AppMetricsTest.Console;

internal class TimerTest : IDisposable
{
    private static readonly TimerOptions RequestTimer = new()
    {
        Name = "Request Timer",
        MeasurementUnit = Unit.Requests,
        DurationUnit = TimeUnit.Milliseconds,
        RateUnit = TimeUnit.Milliseconds
    };

    private readonly AppMetricsTaskScheduler _scheduler;

    private readonly IMetricsRoot _metrics;

    public TimerTest()
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

    public void RequestTimer_Test()
    {
        _metrics.Measure.Timer.Time(RequestTimer, () => Thread.Sleep(500));

        //"timers": [
        //    {
        //        "activeSessions": 0,
        //        "count": 1,
        //        "durationUnit": "ms",
        //        "histogram": {
        //            "lastValue": 522.1075999999999,
        //            "max": 522.1075999999999,
        //            "mean": 522.1075999999999,
        //            "median": 522.1075999999999,
        //            "min": 522.1075999999999,
        //            "percentile75": 522.1075999999999,
        //            "percentile95": 522.1075999999999,
        //            "percentile98": 522.1075999999999,
        //            "percentile99": 522.1075999999999,
        //            "percentile999": 522.1075999999999,
        //            "sampleSize": 1,
        //            "stdDev": 0,
        //            "sum": 522.1075999999999
        //        },
        //        "rate": {
        //            "fifteenMinuteRate": 0,
        //            "fiveMinuteRate": 0,
        //            "meanRate": 0.0009381561816142779,
        //            "oneMinuteRate": 0
        //        },
        //        "rateUnit": "ms",
        //        "name": "Request Timer|server:DESKTOP-6IBBCA1,app:AppMetricsTest.Console,env:release",
        //        "tags": {
        //            "server": "DESKTOP-6IBBCA1",
        //            "app": "AppMetricsTest.Console",
        //            "env": "release"
        //        },
        //        "unit": "req"
        //    }
        //],
    }

    public void RequestTimer2_Test()
    {
        using (_metrics.Measure.Timer.Time(RequestTimer))
        {
            Thread.Sleep(500);
        }
    }

    /// <inheritdoc />
    public void Dispose() => _scheduler.Dispose();
}
