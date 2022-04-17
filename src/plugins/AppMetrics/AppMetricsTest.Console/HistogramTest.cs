using App.Metrics;
using App.Metrics.Histogram;
using App.Metrics.ReservoirSampling.SlidingWindow;
using App.Metrics.Scheduling;

namespace AppMetricsTest.Console;

internal class HistogramTest : IDisposable
{
    private static readonly HistogramOptions PostAndPutRequestSize = new()
    {
        Name = "Web Request Post & Put Size", MeasurementUnit = Unit.Bytes
    };

    private static readonly HistogramOptions HistogramOptions = new()
    {
        Name = "Document File Size", MeasurementUnit = Unit.Calls
    };

    private static readonly HistogramOptions PostRequestSizeHistogram = new()
    {
        Name = "POST Size", MeasurementUnit = Unit.Bytes, Reservoir = () => new DefaultSlidingWindowReservoir(2)
    };

    private readonly AppMetricsTaskScheduler _scheduler;

    private readonly IMetricsRoot _metrics;

    public HistogramTest()
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

    public void PostAndPutRequestSize_Test()
    {
        _metrics.Measure.Histogram.Update(PostAndPutRequestSize, 100);
        _metrics.Measure.Histogram.Update(PostAndPutRequestSize, 100);
        _metrics.Measure.Histogram.Update(PostAndPutRequestSize, 100);
        _metrics.Measure.Histogram.Update(PostAndPutRequestSize, 100);
        _metrics.Measure.Histogram.Update(PostAndPutRequestSize, 200);

        //"histograms": [
        //    {
        //        "count": 5,
        //        "sum": 600,
        //        "lastValue": 200,
        //        "max": 200,
        //        "mean": 120,
        //        "median": 100,
        //        "min": 100,
        //        "percentile75": 100,
        //        "percentile95": 200,
        //        "percentile98": 200,
        //        "percentile99": 200,
        //        "percentile999": 200,
        //        "sampleSize": 5,
        //        "stdDev": 40,
        //        "name": "Web Request Post \u0026 Put Size|server:DESKTOP-6IBBCA1,app:AppMetricsTest.Console,env:release",
        //        "tags": {
        //            "server": "DESKTOP-6IBBCA1",
        //            "app": "AppMetricsTest.Console",
        //            "env": "release"
        //        },
        //        "unit": "B"
        //    }   
        //],
    }

    public void HistogramOptions_Test()
    {
        _metrics.Measure.Histogram.Update(HistogramOptions, 10, "client_1");
        _metrics.Measure.Histogram.Update(HistogramOptions, 30, "client_2");

        //"histograms": [
        //    {
        //        "count": 2,
        //        "sum": 40,
        //        "lastUserValue": "client_2",
        //        "lastValue": 30,
        //        "max": 30,
        //        "maxUserValue": "client_2",
        //        "mean": 20,
        //        "median": 30,
        //        "min": 10,
        //        "minUserValue": "client_1",
        //        "percentile75": 30,
        //        "percentile95": 30,
        //        "percentile98": 30,
        //        "percentile99": 30,
        //        "percentile999": 30,
        //        "sampleSize": 2,
        //        "stdDev": 10,
        //        "name": "Document File Size|server:DESKTOP-6IBBCA1,app:AppMetricsTest.Console,env:release",
        //        "tags": {
        //            "server": "DESKTOP-6IBBCA1",
        //            "app": "AppMetricsTest.Console",
        //            "env": "release"
        //        },
        //        "unit": "calls"
        //    }
        //],
    }

    /// <summary>
    /// 采样策略
    /// </summary>
    public void PostRequestSizeHistogram_Test()
    {
        _metrics.Measure.Histogram.Update(PostRequestSizeHistogram, 10);
        _metrics.Measure.Histogram.Update(PostRequestSizeHistogram, 20);
        _metrics.Measure.Histogram.Update(PostRequestSizeHistogram, 30);

        //"histograms": [
        //    {
        //        "count": 3,
        //        "sum": 60,
        //        "lastValue": 30,
        //        "max": 30,
        //        "mean": 25,
        //        "median": 25,
        //        "min": 20,
        //        "percentile75": 30,
        //        "percentile95": 30,
        //        "percentile98": 30,
        //        "percentile99": 30,
        //        "percentile999": 30,
        //        "sampleSize": 2,
        //        "stdDev": 7.0710678118654755,
        //        "name": "POST Size|server:DESKTOP-6IBBCA1,app:AppMetricsTest.Console,env:release",
        //        "tags": {
        //            "server": "DESKTOP-6IBBCA1",
        //            "app": "AppMetricsTest.Console",
        //            "env": "release"
        //        },
        //        "unit": "B"
        //    }
        //],
    }

    /// <inheritdoc />
    public void Dispose() => _scheduler.Dispose();
}
