using System;
using System.Threading.Tasks;

namespace StackExchangeRedisTest.Lock;

internal class RedisLockTest
{
    public static async Task LockAsync_Test()
    {
        var database = DatabaseProvider.GetDatabase();

        var key = "lockkey";

        var redisLock = await database.LockAsync(key, TimeSpan.FromSeconds(10));
        Console.WriteLine(redisLock.HasTaken);
        await redisLock.DisposeAsync();
        //"SUBSCRIBE" "__keyspace@5__:lockkey"
        //"SELECT" "5"
        //"SET" "lockkey" ":lockkey:1" "EX" "30" "NX"
        //"WATCH" "lockkey"
        //"GET" "lockkey"
        //"MULTI"
        //"DEL" "lockkey"
        //"EXEC"

        using var redisLock2 = await database.LockAsync(key, TimeSpan.FromSeconds(10));
        Console.WriteLine(redisLock2.HasTaken);
        //"SELECT" "5"
        //"SET" "lockkey" ":lockkey:2" "EX" "30" "NX"
        //"WATCH" "lockkey"
        //"GET" "lockkey"
        //"MULTI"
        //"DEL" "lockkey"
        //"EXEC"
    }

    public static async Task LockAsync_Wait_Test()
    {
        var database = DatabaseProvider.GetDatabase();

        var key = "lockkey1";

        var t1 = database.LockAsync(key, TimeSpan.FromSeconds(15));

        //wait until timeout
        var t2 = database.LockAsync(key, TimeSpan.FromSeconds(15));

        var lock1 = await t1;
        var lock2 = await t2;

        lock1.Dispose(); //release lock
        Console.WriteLine(lock1.HasTaken);
        //True
        //"SET" "lockkey1" ":lockkey1:1" "EX" "30" "NX"
        //"WATCH" "lockkey1"
        //"GET" "lockkey1"
        //"MULTI"
        //"EXPIRE" "lockkey1" "30"
        //"EXEC"
        //"SELECT" "5"
        //"WATCH" "lockkey1"
        //"GET" "lockkey1"
        //"MULTI"
        //"DEL" "lockkey1"
        //"EXEC"

        lock2.Dispose();
        Console.WriteLine(lock2.HasTaken);
        //False
    }

    public static async Task LockAsync_Concurrency_Test()
    {
    }
}
