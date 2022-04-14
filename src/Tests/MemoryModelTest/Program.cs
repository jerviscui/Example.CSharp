using System.Threading.Tasks;

namespace MemoryModelTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //while (true)
            //{
            //    MemoryReorderingTests.NonVolatile_Test();
            //}

            //new MemoryReorderingTests().NonVolatile__Test();
            //new MemoryReorderingTests().NonVolatile___Test();

            await ArrayPoolTest.ArrayPool_Test();
            //await ArrayPoolTest.Array_Test();
        }
    }
}
