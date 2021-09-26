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

            await ScriptTest.ScriptEvaluate_ReturnBytes_Test();

            var s = DatabaseProvider.Logger.ToString();
            DatabaseProvider.Dispose();
        }
    }
}
