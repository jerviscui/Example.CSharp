

using AppMetricsTest.Console;

//using var scheduler = SimpleTest.ConsoleReportTest();

//await SimpleTest.Test();
//await SimpleTest.ConfigurationTest(); //配置Configure和不配置的区别
//await SimpleTest.DefaultBuilderTest();

//await SimpleTest.ContextNameTest();

//using var gaugeTest = new GaugeTest();
//gaugeTest.ProcessPhysicalMemoryGauge_Test();
//gaugeTest.ProcessPhysicalMemoryGaugeMB_Test();

//using var counterTest = new CounterTest();
//counterTest.SentEmailsCounter_Test();

//using var meterTest = new MeterTest();
//meterTest.CacheHitsMeter_Test();
//meterTest.HttpStatusMeter_Test();

//using var histogramTest = new HistogramTest();
//histogramTest.PostAndPutRequestSize_Test();
//histogramTest.HistogramOptions_Test();
//histogramTest.PostRequestSizeHistogram_Test();

//using var timerTest = new TimerTest();
//timerTest.RequestTimer_Test();
//timerTest.RequestTimer2_Test();

using var apdexTest = new ApdexTest();
apdexTest.SentEmailsCounter_Test();

Console.ReadLine();
