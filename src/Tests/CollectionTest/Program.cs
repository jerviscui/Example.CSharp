namespace CollectionTest;

internal sealed class Program
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
        ImmutableListTest.ImmutableInterlockedTest();
    }

    #endregion

}
