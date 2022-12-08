using System.Collections.Generic;
using System.Net;
using StackExchange.Redis;

namespace StackExchangeRedisTest.Lock;

public static class RedisLockFactory
{
    public static RedisLock CreateLock(IDatabase database, string key)
    {
        TryConfigRedis(database);
        return new RedisLock(database, key);
    }

    private static readonly List<string> Endpoints = new();

    private static void TryConfigRedis(IDatabase database)
    {
        var connection = database.Multiplexer;
        var endPoints = connection.GetEndPoints();

        var except = new List<EndPoint>();
        foreach (var endPoint in endPoints)
        {
            var address = endPoint.ToString()!;
            if (!Endpoints.Contains(address))
            {
                Endpoints.Add(address);
                except.Add(endPoint);
            }
        }

        //测试通过 主从endpoints
        //todo: 未测试 多主集群endpoints
        foreach (var endPoint in except)
        {
            var server = connection.GetServer(endPoint);

            //必需设置 admin mode，StackExchange.Redis.RedisCommandException
            var config = server.ConfigGet("notify-keyspace-events");
            if (config.Length > 0)
            {
                var (_, value) = config[0];
                bool reset = false;
                if (!value.Contains('g'))
                {
                    value += "g";
                    reset = true;
                }
                if (!value.Contains('K'))
                {
                    value += "K";
                    reset = true;
                }
                if (!value.Contains('E'))
                {
                    value += "E";
                    reset = true;
                }

                if (reset)
                {
                    server.ConfigSet("notify-keyspace-events", value);
                }
            }
        }
    }
}
