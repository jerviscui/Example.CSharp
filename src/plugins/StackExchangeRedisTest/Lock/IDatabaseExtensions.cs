using System;
using StackExchange.Redis;

namespace StackExchangeRedisTest.Lock;

public static class IDatabaseExtensions
{
    public static IRedisLock Lock(this IDatabase database, string key, TimeSpan timeout)
    {
        var redisLock = new RedisLock(database, key);

        redisLock.Lock(timeout);

        return redisLock;
    }
}
