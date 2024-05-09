using static CodeAnalysisTest.ExpressionLevelTest;

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

        // ide0042 csharp_style_deconstructed_variable_declaration = true
        var person = GetPersonTuple();
        Console.WriteLine($"{person.name} {person.age}");

        // ide0056 csharp_style_prefer_index_operator = true
        string[] names = { "Archimedes", "Pythagoras", "Euclid" };
        var index = names[names.Length - 1];

        // ide0057 csharp_style_prefer_range_operator = false
        string sentence = "the quick brown fox";
        var sub = sentence.Substring(0, sentence.Length - 4);

        // ide0080 Remove unnecessary suppression operator
        if (i! is string)
        {
            throw new NotImplementedException();
        }

    }

    private static (string name, string age) GetPersonTuple()
    {
        return (name: "Tom", age: "18");
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
    }

    private static int M(E e)
    {
        // IDE0072: Add missing cases
        var a = e switch
        {
            E.A => 0,
            E.B => throw new NotImplementedException(),
            _ => -1,
        };

        return a;
    }
}
