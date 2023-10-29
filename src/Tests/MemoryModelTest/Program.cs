using System.Threading.Tasks;

namespace MemoryModelTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //VolatileTest.Worker_Test();
            //Console.WriteLine();
            //VolatileTest.VolatileWorker_Test();

            while (true)
            {
                MemoryReorderingTests.NonVolatile_Test();
            }

            //MemoryReorderingTests.Volatile_data_Error_Test();
            //MemoryReorderingTests.Volatile_initialized_Success_Test();

            //await ArrayPoolTest.ArrayPool_Test();
            //await ArrayPoolTest.Array_Test();
        }
    }
}
