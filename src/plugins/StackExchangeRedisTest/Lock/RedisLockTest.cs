using System;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchangeRedisTest.Lock;

internal class RedisLockTest
{
    public static void Lock_Test()
    {
        var database = DatabaseProvider.GetDatabase();

        var key = "lockkey";

        var redisLock = database.Lock(key, TimeSpan.FromSeconds(10));
        Console.WriteLine(redisLock.HasTaken);
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
        Console.WriteLine(redisLock2.HasTaken);
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
        Console.WriteLine(redisLock.HasTaken);
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
        Console.WriteLine(redisLock2.HasTaken);
        //False
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

    //public static async Task LockAsync_Test()
    //{
    //    var database = DatabaseProvider.GetDatabase();

    //    var key = "lockkey";

    //    var redisLock = await database.LockAsync(key, TimeSpan.FromSeconds(10));
    //    Console.WriteLine(redisLock.HasTaken);
    //    await redisLock.DisposeAsync();
    //    //"SUBSCRIBE" "__keyspace@5__:lockkey"
    //    //"SELECT" "5"
    //    //"SET" "lockkey" ":lockkey:1" "EX" "30" "NX"
    //    //"WATCH" "lockkey"
    //    //"GET" "lockkey"
    //    //"MULTI"
    //    //"DEL" "lockkey"
    //    //"EXEC"

    //    using var redisLock2 = await database.LockAsync(key, TimeSpan.FromSeconds(10));
    //    Console.WriteLine(redisLock2.HasTaken);
    //    //"SELECT" "5"
    //    //"SET" "lockkey" ":lockkey:2" "EX" "30" "NX"
    //    //"WATCH" "lockkey"
    //    //"GET" "lockkey"
    //    //"MULTI"
    //    //"DEL" "lockkey"
    //    //"EXEC"
    //}

    //public static async Task LockAsync_Wait_Test()
    //{
    //    var database = DatabaseProvider.GetDatabase();

    //    var key = "lockkey1";

    //    var t1 = database.LockAsync(key, TimeSpan.FromSeconds(15));

    //    //wait until timeout
    //    var t2 = database.LockAsync(key, TimeSpan.FromSeconds(15));

    //    var lock1 = await t1;
    //    var lock2 = await t2;

    //    lock1.Dispose(); //release lock
    //    Console.WriteLine(lock1.HasTaken);
    //    //True
    //    //"SET" "lockkey1" ":lockkey1:1" "EX" "30" "NX"
    //    //"WATCH" "lockkey1"
    //    //"GET" "lockkey1"
    //    //"MULTI"
    //    //"EXPIRE" "lockkey1" "30"
    //    //"EXEC"
    //    //"SELECT" "5"
    //    //"WATCH" "lockkey1"
    //    //"GET" "lockkey1"
    //    //"MULTI"
    //    //"DEL" "lockkey1"
    //    //"EXEC"

    //    lock2.Dispose();
    //    Console.WriteLine(lock2.HasTaken);
    //    //False
    //}
}
