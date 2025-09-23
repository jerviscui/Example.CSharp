using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;
using StackExchange.Redis.Extensions.Core.Configuration;
using StackExchange.Redis.Extensions.Core.Implementations;

namespace StackExchangeRedisTest;

public static class ExtensionDatabaseProvider
{

    #region Constants & Statics

    private static readonly IRedisClientFactory Factory;
    private static readonly ILoggerFactory Logger;
    private static readonly RedisConfiguration RedisConfiguration = new()
    {
        AbortOnConnectFail = true,
        Hosts = [new() { Host = "localhost", Port = 6379 }],
        Database = 5,
        //Ssl = true,
        AllowAdmin = true,
        KeyPrefix = "ExtensionTest:",
        //MaxValueLength = 1024,
        IsDefault = true,
        Name = "ExtensionTest",
    };

    static ExtensionDatabaseProvider()
    {
        Logger = LoggerFactory.Create((builder) => builder.AddConsole().SetMinimumLevel(LogLevel.Information));
        Factory = new RedisClientFactory([RedisConfiguration], Logger, new RedisMemoryPackSerializer());
    }

    public static void Dispose()
    {
        Logger.Dispose();
        foreach (var item in Factory.GetAllClients())
        {
            item.ConnectionPoolManager.Dispose();
        }
    }

    /// <summary>
    /// Gets the database.
    /// </summary>
    public static IRedisDatabase GetDatabase()
    {
        return Factory.GetDefaultRedisDatabase();
    }

    public static void Start()
    {
        //clear
        //GetDatabase().Execute("flushdb");
    }

    #endregion

}
