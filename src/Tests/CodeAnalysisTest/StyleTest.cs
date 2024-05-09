namespace CodeAnalysisTest;

internal sealed class StyleTest
{
    public required string S { get; set; }

    public void Test(string? s)
    {
        // ide0016 csharp_style_throw_expression = true
        if (s == null)
        {
            throw new ArgumentNullException(nameof(s));
        }
        S = s;

        // ide0018 csharp_style_inlined_variable_declaration = true
        int i;
        if (int.TryParse(s, out i))
        {
            int y = i;
        }
    }

    // ide0034 csharp_prefer_simple_default_expression = true
    private static void DoWork(CancellationToken cancellationToken = default(CancellationToken))
    {
        // ide0039 csharp_style_prefer_local_over_anonymous_function = false
        // 匿名方法
        //Func<int, int> doubleFunc = delegate (int x) { return x * 2; };
        static int doubleFunc(int x) { return x * 2; }
        // Lambda 表达式
        //Func<int, int> doubleLambda = (x) => { return x * 2; };
        static int doubleLambda(int x) { return x * 2; }

        throw new NotSupportedException();
    }
}
