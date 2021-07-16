namespace MemoryModelTest
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                new MemoryReorderingTests().NonVolatile_Test();
            }

            //new MemoryReorderingTests().NonVolatile__Test();
            //new MemoryReorderingTests().NonVolatile___Test();
        }
    }
}
