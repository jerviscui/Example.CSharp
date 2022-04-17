// See https://aka.ms/new-console-template for more information

using AppMetricsTest.Console;

//using var scheduler = SimpleTest.ConsoleReportTest();

//await SimpleTest.Test();
//await SimpleTest.ConfigurationTest(); //配置Configure和不配置的区别
//await SimpleTest.DefaultBuilderTest();

//await SimpleTest.ContextNameTest();

//using var gaugeTest = new GaugeTest();
//gaugeTest.ProcessPhysicalMemoryGauge_Test();
//gaugeTest.ProcessPhysicalMemoryGaugeMB_Test();

using var counterTest = new CounterTest();
counterTest.SentEmailsCounter_Test();

Console.ReadLine();
