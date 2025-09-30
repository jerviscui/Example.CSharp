namespace MemoryModelTest;

internal static class Program
{

    #region Constants & Statics

    public static void Main(string[] args)
    {
        //VolatileTest.Worker_Test();
        //Console.WriteLine();
        //VolatileTest.VolatileWorker_Test();

        //while (true)
        //{
        //    MemoryReorderingTests.NonVolatile_Test();
        //}

        //MemoryReorderingTests.Volatile_data_Error_Test();
        //MemoryReorderingTests.Volatile_initialized_Success_Test();

        //await ArrayPoolTest.ArrayPool_Test();
        //await ArrayPoolTest.Array_Test();

        //CacheLineTest.SharedCacheLine_Test();
        //CacheLineTest.ExcCacheLine_Test();
        //Console.WriteLine();
        //CacheLineTest.SharedCacheLine_Test();
        //CacheLineTest.ExcCacheLine_Test();

        //ArrayBufferWriterTest.Test();

        //ArrayPoolBufferWriterTest.Test();

        //MemoryOwnerTest.SliceTest();

        SpanOwnerTest.Test();
    }

    #endregion

}
