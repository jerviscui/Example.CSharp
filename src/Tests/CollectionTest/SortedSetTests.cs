namespace CollectionTest;

internal sealed class SortedSetTests
{
    public static void Order_Test()
    {
        var set = new SortedSet<int>();
        set.Add(1);
        set.Add(3);
        set.Add(2);
        set.Add(3);

        Console.WriteLine($"count: {set.Count}");
        foreach (var i in set)
        {
            Console.WriteLine(i);
        }

        //Set 不存储重复项
        //count: 3
        //1
        //2
        //3
    }
}
