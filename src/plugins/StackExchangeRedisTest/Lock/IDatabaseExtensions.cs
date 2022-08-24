using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace StackExchangeRedisTest.Lock;

public static class IDatabaseExtensions
{
    public static IRedisLockOnce Lock(this IDatabase database, string key, TimeSpan timeout)
    {
        var redisLock = new RedisLock(database, key);

        _ = redisLock.Lock(timeout);

        return redisLock;
    }

    public static async Task<IRedisLockOnce> LockAsync(this IDatabase database, string key, TimeSpan timeout)
    {
        var redisLock = new RedisLock(database, key);

        _ = await redisLock.LockAsync(timeout);

        return redisLock;
    }
}
