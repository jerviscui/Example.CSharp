namespace StackExchangeRedisTest;

internal static class Program
{

    #region Constants & Statics

    private static async Task Main(string[] args)
    {
        DatabaseProvider.Start();

        //await StorageTest.StorageString_Test();
        //await StorageTest.StorageBytes_Test();

        //await SerializeTest.SystemTextJson_Test();
        //await SerializeTest.NewtownJson_Test();

        //await ScriptTest.ScriptEvaluate_ReturnBytes_Test();

        //await TransactionTest.Transaction_Test();
        //await TransactionTest.ChangeWatchValue_Test();
        //await TransactionTest.MultiCondition_Test();

        //LockTest.LockTakeAsync_Test();
        //LockTest.LockExtend_Test();
        //LockTest.LockTakeAsync_FailedWithBlock_Test();

        //HashTest.HashSet_Test();
        //HashTest.HashSet_WithHashEntry_Test();
        //HashTest.HashGet_Test();
        //HashTest.HashGetAll_Test();
        //HashTest.HashGetLease_Test();

        //await ProfilingTest.ProfilingSession_Test();
        //await ProfilingTest.ProfilingSession_MultiThread_Test();

        //RedisLockTest.Lock_Test();
        //RedisLockTest.Lock_DeadLock_Test();
        //RedisLockTest.Lock_OnlyOnce_Test();
        //await RedisLockTest.Lock_Concurrency_Test();
        //await RedisLockTest.LockAsync_DeadLock_Test();
        //await RedisLockTest.LockAsync_Concurrency_Test();

        //TimeTest.TimeToDateTime_Test();

        var s = DatabaseProvider.Logger.ToString();
        DatabaseProvider.Dispose();

        ExtensionDatabaseProvider.Start();

        //await RedisExtensionsTest.MemoryPackSerializer_TestAsync();
        //await RedisExtensionsTest.MemoryPackSerializer_Null_TestAsync();
        //await RedisExtensionsTest.MaxLength_TestAsync();
        await RedisExtensionsTest.Tagging_TestAsync();

        ExtensionDatabaseProvider.Dispose();
    }

    #endregion

}
