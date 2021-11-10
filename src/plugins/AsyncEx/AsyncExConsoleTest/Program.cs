using System.Threading.Tasks;

namespace AsyncExConsoleTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //AsyncLockTest.AsyncLock_UsedSync_Test();
            //await AsyncLockTest.AsyncLock_UsedAsync_Test();
            await AsyncLockTest.AsyncLock_UsedAsync_WithMethod_Test();
        }
    }
}
