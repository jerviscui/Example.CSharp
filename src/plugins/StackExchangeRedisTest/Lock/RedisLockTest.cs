using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchangeRedisTest.Lock;

internal sealed class RedisLockTest
{
    public static void Lock_Test()
    {
        var database = DatabaseProvider.GetDatabase();

        var key = "lockkey";

        var redisLock = database.Lock(key, TimeSpan.FromSeconds(10));
        Console.WriteLine(redisLock.IsLocked);
        redisLock.Dispose();
        //True
        //"SUBSCRIBE" "__keyspace@5__:lockkey"
        //"SELECT" "5"
        //"SET" "lockkey" ":lockkey:1" "EX" "30" "NX"
        //"WATCH" "lockkey"
        //"GET" "lockkey"
        //"MULTI"
        //"DEL" "lockkey"
        //"EXEC"

        var redisLock2 = database.Lock(key, TimeSpan.FromSeconds(10));
        Console.WriteLine(redisLock2.IsLocked);
        redisLock2.Dispose();
        //True
        //"SELECT" "5"
        //"SET" "lockkey" ":lockkey:2" "EX" "30" "NX"
        //"WATCH" "lockkey"
        //"GET" "lockkey"
        //"MULTI"
        //"DEL" "lockkey"
        //"EXEC"
    }

    public static void Lock_DeadLock_Test()
    {
        var database = DatabaseProvider.GetDatabase();

        var key = "lockkey1";

        using var redisLock = database.Lock(key, TimeSpan.FromSeconds(5));
        Console.WriteLine(redisLock.IsLocked);
        //True
        //"SUBSCRIBE" "__keyspace@5__:lockkey1"
        //"SELECT" "5"
        //"SET" "lockkey1" ":lockkey:1" "EX" "30" "NX"
        //"WATCH" "lockkey1"
        //"GET" "lockkey1"
        //"MULTI"
        //"DEL" "lockkey1"
        //"EXEC"

        //using redisLock 还未释放，死锁
        using var redisLock2 = database.Lock(key, TimeSpan.FromSeconds(5));
        Console.WriteLine(redisLock2.IsLocked);
        //False
    }

    public static void Lock_OnlyOnce_Test()
    {
        var database = DatabaseProvider.GetDatabase();

        var key = "lockkey3";

        using var redisLock = (IRedisLock)database.Lock(key, TimeSpan.FromSeconds(5));
        try
        {
            redisLock.Lock(TimeSpan.FromSeconds(5));
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    public static async Task Lock_Concurrency_Test()
    {
        var database = DatabaseProvider.GetDatabase();
        var key = "lockkey2";

        var tasks = new Task[10];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = new Task(() =>
            {
                var redisLock = database.Lock(key, Timeout.InfiniteTimeSpan);
                Thread.Sleep(1000);
                redisLock.Dispose();
            });
        }

        foreach (var task in tasks)
        {
            task.Start();
        }

        await Task.WhenAll(tasks);
    }

    public static async Task LockAsync_DeadLock_Test()
    {
        var database = DatabaseProvider.GetDatabase();

        var key = "lockasynckey";

        var t1 = database.LockAsync(key, TimeSpan.FromSeconds(5));

        var t2 = database.LockAsync(key, TimeSpan.FromSeconds(5));

        await using var redisLock1 = await t1;
        Console.WriteLine(redisLock1.IsLocked);

        await using var redisLock2 = await t2;
        Console.WriteLine(redisLock2.IsLocked);
    }

    public static async Task LockAsync_Concurrency_Test()
    {
        var database = DatabaseProvider.GetDatabase();
        var key = "lockasynckey1";

        var tasks = new Task[10];
        for (int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = new Task(() =>
            {
                var redisLock = database.LockAsync(key, TimeSpan.FromSeconds(7) /*Timeout.InfiniteTimeSpan*/)
                    .ConfigureAwait(false)
                    .GetAwaiter().GetResult();
                Console.WriteLine($"{key} {redisLock.IsLocked}");
                Thread.Sleep(1000);
                redisLock.Dispose();
            });
        }

        foreach (var task in tasks)
        {
            task.Start();
        }

        await Task.WhenAll(tasks);
    }
}
