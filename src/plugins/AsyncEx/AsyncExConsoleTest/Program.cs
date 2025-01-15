using System.Threading.Tasks;

namespace AsyncExConsoleTest
{
    internal sealed class Program
    {
        private static async Task Main(string[] args)
        {
            //AsyncLockTest.AsyncLock_UsedSync_Test();
            await AsyncLockTest.AsyncLock_UsedAsync_Test();
            //await AsyncLockTest.AsyncLock_UsedAsync_WithMethod_Test();

            AsyncContextTest.Run_Task_Test();
            //AsyncContextTest.Run_Test();

            //AsyncReaderWriterLockTest.ReaderWriterLock_Test();
        }
    }
}
