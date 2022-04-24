namespace CollectionTest;

internal class SortedListTests
{
    public static void Order_Test()
    {
        var list = new SortedList<int, string>();
        list.Add(1, "1");
        list.Add(3, "1");
        list.Add(2, "1");
        //list.Add(3, "1"); //throw System.ArgumentException:“An item with the same key has already been added. Key: 3 Arg_ParamName_Name”

        Console.WriteLine($"count: {list.Count}");
        foreach (var i in list)
        {
            Console.WriteLine(i);
        }
    }
}
