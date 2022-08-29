using System;

namespace TypeHandleTest;

public class ReflectedTypeTest
{
    public static void OverrideMethod_Test()
    {
        var method = typeof(MyClass).GetMethod(nameof(MyClass.OverrideMethod))!;
        var method2 = typeof(MyClass2).GetMethod(nameof(MyClass.OverrideMethod))!;

        Console.WriteLine(method.DeclaringType);
        Console.WriteLine(method.ReflectedType);

        Console.WriteLine(method2.DeclaringType);
        Console.WriteLine(method2.ReflectedType);

        //TypeHandleTest.MyClass
        //TypeHandleTest.MyClass
        //TypeHandleTest.MyClass2
        //TypeHandleTest.MyClass2
    }

    public static void DerivedMethod_Test()
    {
        var method = typeof(MyClass).GetMethod(nameof(MyClass.BaseMethod))!;
        var method2 = typeof(MyClass2).GetMethod(nameof(MyClass.BaseMethod))!;

        Console.WriteLine(method.DeclaringType);
        Console.WriteLine(method.ReflectedType);

        Console.WriteLine(method2.DeclaringType);
        Console.WriteLine(method2.ReflectedType);

        //TypeHandleTest.MyClass
        //TypeHandleTest.MyClass
        //TypeHandleTest.MyClass
        //TypeHandleTest.MyClass2
    }
}

public class MyClass
{
    public virtual void OverrideMethod()
    {
    }

    public void BaseMethod()
    {
    }
}

public class MyClass2 : MyClass
{
    public override void OverrideMethod()
    {
    }
}
