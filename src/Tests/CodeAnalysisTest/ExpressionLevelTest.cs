namespace CodeAnalysisTest;

internal sealed class ExpressionLevelTest
{

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

        // ide0037 dotnet_style_prefer_inferred_anonymous_type_member_names = true ,样式已移除只保留重构
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

        return y;
    }

    private static void M()
    {
        throw new System.Exception();

        // IDE0035: Remove unreachable code
        //var v = 0;
    }

    private static (string name, int age) GetCustomer()
    {
        throw new NotImplementedException();
    }

    private enum E
    {
        A,
        B
    }
}



internal class Unused
{
    // IDE0051: Remove unused private members
    private readonly int _fieldPrivate;
    private static int PropertyPrivate => 1;
    private static int GetNumPrivate()
    {
        return 1;
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
