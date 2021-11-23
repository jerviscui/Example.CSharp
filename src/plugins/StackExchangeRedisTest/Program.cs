using System.Threading.Tasks;

namespace StackExchangeRedisTest
{
    class Program
    {
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

            //LockTest.LockTakeAsync_Test();
            //LockTest.LockExtend_Test();
            //LockTest.LockTakeAsync_FailedWithBlock_Test();

            //HashTest.HashSet_Test();
            //HashTest.HashSet_WithHashEntry_Test();
            HashTest.HashGet_Test();
            HashTest.HashGetAll_Test();
            HashTest.HashGetLease_Test();

            var s = DatabaseProvider.Logger.ToString();
            DatabaseProvider.Dispose();
        }
    }
}
