namespace CodeAnalysisTest;


internal sealed class MyClass
{
    public MyClass(int i) // IDE0290 csharp_style_prefer_primary_constructors
    {
    }

    public MyClass()
    {
    }


    public int Age { get; set; }

    internal static readonly int[] sourceArray = new[] { 1, 2, 3 };

    private static void Test()
    {
        // ide0200 Code with violations.
        bool IsEven(int x) => x % 2 == 0;
        _ = sourceArray.Where(o => IsEven(o));
    }
}
