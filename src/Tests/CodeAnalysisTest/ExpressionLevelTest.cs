using System.Globalization;

namespace CodeAnalysisTest;

internal sealed class ExpressionLevelTest
{

    public static void Test()
    {
        M1();
        M2(E.B);
        M3();
        M4();
        M5();
    }

    private static void M1()
    {
        throw new NotSupportedException();
    }

    private static string M2(E e)
    {
        // IDE0002: 'C.M1' can be simplified to 'M1'
        ExpressionLevelTest.M1();

        // ide0004 Remove unnecessary cast
        int v = (int)0;

        // IDE0010: Add missing cases
        switch (e)
        {
            case E.A:
                return "";
            case E.B:
            default:
                break;
        }

        // ide0017 otnet_style_object_initializer = true
        var my = new MyClass();
        my.Age = 21;

        // ide0028 dotnet_style_prefer_collection_expression = true 
        List<int> list = new List<int>();
        list.Add(1);
        list.Add(2);
        list.Add(3);

        // IDE0029 dotnet_style_coalesce_expression = true
        var x = Random.Shared.Next() > 0 ? null : my;
        var y = my;
        var xy = x != null ? x : y;

        // IDE0270 dotnet_style_coalesce_expression = true
        var item = new object() as MyClass;
        if (item == null)
        {
            throw new InvalidOperationException();
        }

        // ide0031 dotnet_style_null_propagation = true
        int? o = Random.Shared.Next() > 0 ? null : 1;
        var vv = o != null ? o.ToString() : null;

        // ide0033 dotnet_style_explicit_tuple_names = true
        (string name, int age) customer = GetCustomer();
        var name = customer.Item1;

        // ide0037 dotnet_style_prefer_inferred_tuple_names = true
        string? age = Random.Shared.Next() > 0 ? null : "";
        var tuple = (age: age, name: name);

        // ide0037 dotnet_style_prefer_inferred_anonymous_type_member_names = true
        var anon = new { age = age, name = name };

        // ide0041 dotnet_style_prefer_is_null_check_over_reference_equality_method = true
        if (ReferenceEquals(age, null))
        {
            return "";
        }

        // ide0045 dotnet_style_prefer_conditional_expression_over_assignment = true
        var expr = Random.Shared.Next() > 0;
        if (expr)
        {
            name = "hello";
        }
        else
        {
            name = "world";
        }

        // ide0046 dotnet_style_prefer_conditional_expression_over_return = true
        if (expr)
        {
            return "hello";
        }
        else
        {
            return "world";
        }
    }

    private static int M3()
    {
        // ide0050 Convert anonymous type to tuple，样式已移除，只保留重构
        var t1 = new { a = 1, b = 2 };

        // ide0054 dotnet_style_prefer_compound_assignment = true
        int x = 1;
        x = x + 5;

        // ide0058 csharp_style_unused_value_expression_statement_preference = discard_variable
        Convert.ToInt32("35");

        // IDE0059: csharp_style_unused_value_assignment_preference = discard_variable
        int v = Random.Shared.Next();
        v = Random.Shared.Next();
        int y = v;

        // ide0071 dotnet_style_prefer_simplified_interpolation = true
        var str = $"prefix {y.ToString()} suffix";

        // ide0075 dotnet_style_prefer_simplified_boolean_expressions = true
        var result1 = M4() && M5() ? true : false;
        var result2 = M4() ? true : M5();

        // ide0082 Convert typeof to nameof
        var n1 = typeof(ExpressionLevelTest).Name;
        var n2 = typeof(int).Name;

        // ide0100 Remove unnecessary equality operator
        if (result1 == true)
        {
            y = 0;
        }
        if (M4() != false)
        {
            y = 1;
        }

        // ide0120
        IEnumerable<string> words = new List<string> { "hello", "world", "!" };
        var result = words.Where(x => x.Equals("hello")).Any();

        return y;
    }
    private static bool M5()
    {
        throw new NotImplementedException();
    }
    private static bool M4()
    {
        throw new NotImplementedException();
    }

    private static void M()
    {
        throw new NotImplementedException();

        // IDE0035: Remove unreachable code
        //var v = 0;
    }

    private static (string name, int age) GetCustomer()
    {
        throw new NotImplementedException();
    }

    public enum E
    {
        A,
        B
    }

    private readonly int j = 1;

    // IDE0070: GetHashCode can be simplified.
    public override int GetHashCode()
    {
        var hashCode = 339610899;
        hashCode = hashCode * -1521134295 + base.GetHashCode();
        hashCode = hashCode * -1521134295 + j.GetHashCode();
        return hashCode;
    }

    public override bool Equals(object? obj)
    {
        return obj is ExpressionLevelTest test &&
               j == test.j;
    }
}



internal sealed class Unused
{
    // IDE0051: Remove unused private members
    private readonly int _fieldPrivate;
    private static int PropertyPrivate => 1;
    private static int GetNumPrivate()
    {
        return DateTime.Now.Second;
    }

    // IDE0052: Remove unread private members
    private readonly int _field1;
    private int _field2;
    private int Property { get; set; }

    public Unused()
    {
        _field1 = 0;
    }

    public void SetMethod()
    {
        _field2 = 0;
        Property = 0;
    }
}
