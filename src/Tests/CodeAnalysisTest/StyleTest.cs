using System.Collections.Immutable;
using static CodeAnalysisTest.ExpressionLevelTest;

namespace CodeAnalysisTest;

internal sealed class StyleTest
{
    public required string S { get; set; }

    internal static readonly int[] items = new int[] { 5, 6, 7 };

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

        // ide0300 dotnet_style_prefer_collection_expression = when_types_loosely_match
        string[] names = { "Archimedes", "Pythagoras", "Euclid" };

        // ide0301 dotnet_style_prefer_collection_expression = when_types_loosely_match
        int[] arr = Array.Empty<int>();

        // ide0302 dotnet_style_prefer_collection_expression = when_types_loosely_match
        ReadOnlySpan<int> x = stackalloc int[] { 1, 2, 3 };

        // ide0303 dotnet_style_prefer_collection_expression = when_types_loosely_match
        ImmutableArray<int> arr2 = ImmutableArray.Create(1, 2, 3);

        // ide0304 dotnet_style_prefer_collection_expression = when_types_loosely_match
        var builder = ImmutableArray.CreateBuilder<int>();
        builder.Add(1);
        builder.AddRange(items);
        ImmutableArray<int> arr3 = builder.ToImmutable();

        // ide0305 dotnet_style_prefer_collection_expression = when_types_loosely_match
#pragma warning disable CA1861 // Avoid constant arrays as arguments
        List<int> arr4 = new[] { 1, 2, 3 }.ToList();
#pragma warning restore CA1861 // Avoid constant arrays as arguments

        // ide0056 csharp_style_prefer_index_operator = true
        var index = names[names.Length - 1];

        // ide0057 csharp_style_prefer_range_operator = true
        string sentence = "the quick brown fox";
        var sub = sentence.Substring(0, sentence.Length - 4);

        // ide0080 Remove unnecessary suppression operator
        // ide0150 csharp_style_prefer_null_check_over_type_check = true
        var o = "";
        if (o! is string)
        {
            throw new NotImplementedException();
        }

        // ide0090 csharp_style_implicit_object_creation_when_type_is_apparent = true
        var list = new List<object>() { new object() };

        // ide0110 Code with violations
        o = Random.Shared.Next() > 1 ? "" : null;
        switch (o)
        {
            case string _:
                Console.WriteLine("Value was a string");
                break;
            default:
                break;
        }

        // ide0180 csharp_style_prefer_tuple_swap = true
        List<int> numbers = [5, 6, 4];
        int temp = numbers[0];
        numbers[0] = numbers[1];
        numbers[1] = temp;

        // ide0220 dotnet_style_prefer_foreach_explicit_cast_in_source = when_strongly_typed
        //foreach (string item in list)
        foreach (string item in list.Cast<string>())
        {
            Console.WriteLine(item);
        }

        // ide0230 csharp_style_prefer_utf8_string_literals = true
        ReadOnlySpan<byte> _ = new byte[] { 65, 66, 67 };



        // ide0240 Nullable directive is redundant
#nullable enable

        // ide0241 Nullable directive is unnecessary
#nullable disable
#nullable disable
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
        int b = a;


        return b;


    }
}

