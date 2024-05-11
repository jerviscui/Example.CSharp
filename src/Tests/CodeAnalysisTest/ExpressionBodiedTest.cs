namespace CodeAnalysisTest;

internal sealed class ExpressionBodiedTest
{
    public int Age { get; private set; }

    // ide0021 csharp_style_expression_bodied_constructors = false
    public ExpressionBodiedTest(int age)
    {
        Age = age;
        _age = 0;
    }

    // ide0022 csharp_style_expression_bodied_methods = false
    public int GetAge()
    {
        return this.Age;
    }

    // ide0024 csharp_style_expression_bodied_operators = true
    public static ExpressionBodiedTest operator +(ExpressionBodiedTest c1, ExpressionBodiedTest c2)
    {
        return new ExpressionBodiedTest(c1.Age + c2.Age);
    }

    // ide0025 csharp_style_expression_bodied_properties = true
    private int _age;
    public int Age2
    {
        get { return _age; } // ide0027 csharp_style_expression_bodied_accessors = true

    }

    // ide0026 csharp_style_expression_bodied_indexers = true
    public int this[int i] { get { return _age; } }

    // ide0053 csharp_style_expression_bodied_lambdas = true
    private readonly Func<int, int> square = x => { return x * x; };

    private static void M()
    {
        Hello();

        // ide0061 csharp_style_expression_bodied_local_functions = true
        void Hello()
        {
            Console.WriteLine("Hello");
        }
    }
}
