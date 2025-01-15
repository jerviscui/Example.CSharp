using App.Metrics;
using App.Metrics.Apdex;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.Scheduling;

namespace AppMetricsTest.Console;

internal sealed class ApdexTest : IDisposable
{
    private static readonly ApdexOptions Apdex = new()
    {
        Name = "Sample Apdex",
        ApdexTSeconds = 0.1, // Adjust based on your SLA
        Reservoir = () => new DefaultSlidingWindowReservoir(3)
    };

    private readonly AppMetricsTaskScheduler _scheduler;

    private readonly IMetricsRoot _metrics;

    public ApdexTest()
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

    public void SentEmailsCounter_Test()
    {
        using (_metrics.Measure.Apdex.Track(Apdex))
        {
            Thread.Sleep(100);
        }

        using (_metrics.Measure.Apdex.Track(Apdex))
        {
            Thread.Sleep(400);
        }

        using (_metrics.Measure.Apdex.Track(Apdex))
        {
            Thread.Sleep(300);
        }

        //"apdexScores": [
        //    {
        //        "frustrating": 1,
        //        "sampleSize": 3,
        //        "satisfied": 0,
        //        "score": 1,
        //        "tolerating": 2,
        //        "name": "Sample Apdex|server:DESKTOP-6IBBCA1,app:AppMetricsTest.Console,env:release",
        //        "tags": {
        //            "server": "DESKTOP-6IBBCA1",
        //            "app": "AppMetricsTest.Console",
        //            "env": "release"
        //        }
        //    }
        //],
    }

    /// <inheritdoc />
    public void Dispose() => _scheduler.Dispose();
}
