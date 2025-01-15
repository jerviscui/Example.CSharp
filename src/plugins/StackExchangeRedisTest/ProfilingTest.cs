using System;
using System.Text.Json;
using System.Threading.Tasks;
using StackExchange.Redis;
using StackExchange.Redis.Profiling;

namespace StackExchangeRedisTest;

internal interface IRedisProfiler
{
    public ProfilingSession GetSession();
}

internal sealed class RedisProfiler : IRedisProfiler
{
    private readonly ProfilingSession _profilingSession;

    public RedisProfiler()
    {
        _profilingSession = new ProfilingSession();
    }

    public ProfilingSession GetSession() => _profilingSession;
}

internal sealed class ProfilingTest
{
    public static async Task ProfilingSession_Test()
    {
        var conn = DatabaseProvider.Connection;
        var database = conn.GetDatabase();

        var redisProfiler = new RedisProfiler();
        conn.RegisterProfiler(redisProfiler.GetSession);

        await database.StringSetAsync("test", "testvalue", TimeSpan.FromMinutes(1), When.NotExists);

        await Task.Delay(1000);

        var profiling = redisProfiler.GetSession().FinishProfiling();
        foreach (var command in profiling)
        {
            Console.WriteLine(JsonSerializer.Serialize(command));
        }
    }

    public static async Task ProfilingSession_MultiThread_Test()
    {
        var conn = DatabaseProvider.Connection;
        var database = conn.GetDatabase();

        var redisProfiler = new RedisProfiler();
        conn.RegisterProfiler(redisProfiler.GetSession);

        var t = Task.Run(async () =>
        {
            while (true)
            {
                await database.StringSetAsync("test", "testvalue", TimeSpan.FromMinutes(1), When.NotExists);

                await Task.Delay(1000);

                var profiling = redisProfiler.GetSession().FinishProfiling();
                foreach (var command in profiling)
                {
                    Console.WriteLine(JsonSerializer.Serialize(command));
                }
            }
        });

        await t;
    }
}
