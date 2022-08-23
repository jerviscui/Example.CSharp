using System;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace StackExchangeRedisTest.Lock;

public static class IDatabaseExtensions
{
    public static async Task<IRedisLock> LockAsync(this IDatabase database, string key, TimeSpan timeout)
    {
        var redisLock = new RedisLock(database, key);

        await redisLock.LockAsync(timeout);

        return redisLock;
    }
}
