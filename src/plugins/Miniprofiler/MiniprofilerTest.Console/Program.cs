using StackExchange.Profiling;
using StackExchange.Profiling.Internal;
using StackExchange.Profiling.SqlFormatters;
using StackExchange.Profiling.Storage;

MiniProfiler.Configure(new MiniProfilerBaseOptions
{
    EnableDebugMode = true,
    Storage = new RedisStorage("10.99.59.47:7000,DefaultDatabase=3"),
    SqlFormatter = new VerboseSqlServerFormatter(true)
});

var profiler = MiniProfiler.StartNew("Console.Program");

using (var step = profiler.Step("first"))
{
    Thread.Sleep(1000);
}

Console.WriteLine(profiler.RenderPlainText());
