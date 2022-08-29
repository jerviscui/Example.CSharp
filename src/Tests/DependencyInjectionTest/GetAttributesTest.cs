using System.Reflection;

namespace DependencyInjectionTest;

public class GetAttributesTest
{
    public static void GetCustomAttributes_Test()
    {
        var method = typeof(MyClass).GetMethod(nameof(IFoo.Test))!;
        var list = method.GetCustomAttributes<MyAttribute>(true);

        foreach (var attribute in list)
        {
            Console.WriteLine(attribute.MyProperty);
        }

        //output:
        //2
        //3
    }

    public static void GetCustomAttributes_InheritChain_Test()
    {
        var method = typeof(MyClass3).GetMethod(nameof(IFoo.Test))!;
        var list = method.GetCustomAttributes<MyAttribute>(true);

        foreach (var attribute in list)
        {
            Console.WriteLine(attribute.MyProperty);
        }

        //output:
        //5
        //4
        //2
        //3
    }

    public static void GetTypeAttributes_Test()
    {
        var type = typeof(MyClass3);

        var list = type.GetCustomAttributes<MyAttribute>(true);

        foreach (var attribute in list)
        {
            Console.WriteLine(attribute.MyProperty);
        }

        //output:
        //44
        //33
        //22
    }

    public static void GetInterfaceAttributes_Test()
    {
        var method = typeof(IFoo).GetMethod(nameof(IFoo.Test))!;
        var list = method.GetCustomAttributes<MyAttribute>(true);

        foreach (var attribute in list)
        {
            Console.WriteLine(attribute.MyProperty);
        }

        //output:
        //1
    }
}

[My(11)]
public interface IFoo
{
    [My(1)]
    public void Test();
}

[My(22)]
public class MyClass : IFoo
{
    [My(2)]
    [My(3)]
    public virtual void Test()
    {
    }
}

[My(33)]
public class MyClass2 : MyClass
{
    [My(4)]
    public override void Test()
    {
    }
}

[My(44)]
public class MyClass3 : MyClass2
{
    [My(5)]
    public override void Test()
    {
    }
}

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
internal class MyAttribute : Attribute
{
    public int MyProperty { get; set; }

    public MyAttribute(int prop) => MyProperty = prop;
}
