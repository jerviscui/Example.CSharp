namespace CollectionTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        //SortedSetTests.Order_Test();

        //SortedListTests.Order_Test();

        //ConditionalWeakTableTests.Test();

        //Console.ReadLine();

        //BenchmarkRunner.Run<FrozenCollectionBenchmark>();

        //ImmutableListTest.Test();
        //ImmutableListTest.CopyOnWriteTest();
        //ImmutableListTest.ImmutableInterlockedTest();
        ImmutableListTest.RefCopy_Test();
        ImmutableListTest.RefChange_Test();

        var i = 0;
        while (true)
        {
            var s = $"abcdefghijklopqrstuvwxyz{i}";
            i++;
            Thread.Sleep(100);
        }
    }

    #endregion

}
