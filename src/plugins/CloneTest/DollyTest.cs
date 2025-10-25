namespace CloneTest;

public static class DollyTest
{

    #region Constants & Statics

    public static void ComplexClassTest()
    {
        var a = new ComplexClass(
            new SimpleRecord("Test", 42) { DontClone = 3.14f },
            new SimpleClass { First = "Test", Second = 42, DontClone = 3.14f })
        {
            Third = 7
        };

        var b = a.DeepClone();

        Console.WriteLine($"{b.SimpleRecord.First} {b.SimpleClass.DontClone} {b.Third}");
    }

    public static void SimpleClassTest()
    {
        var a = new SimpleClass { First = "Test", Second = 42, DontClone = 3.14f };

        var b = a.DeepClone();

        Console.WriteLine($"{b.First} {b.DontClone}");
    }

    public static void SimpleRecordTest()
    {
        var a = new SimpleRecord("Test", 42) { DontClone = 3.14f };

        var b = a.DeepClone();

        Console.WriteLine($"{b.First} {b.DontClone}");
    }

    #endregion

}
