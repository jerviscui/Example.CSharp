namespace MemoryModelTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            while (true)
            {
                MemoryReorderingTests.NonVolatile_Test();
            }

            //new MemoryReorderingTests().NonVolatile__Test();
            //new MemoryReorderingTests().NonVolatile___Test();
        }
    }
}
