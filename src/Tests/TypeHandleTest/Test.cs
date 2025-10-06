using System;
using System.Reflection;

namespace TypeHandleTest;

[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
public class MyAttribute : Attribute
{
    /// <inheritdoc/>
    public MyAttribute(int count)
    {
        Count = count;
    }

    #region Properties

    public int Count { get; set; }

    #endregion

    #region Methods

    public void Do()
    {
        Console.WriteLine($"MyAttribute {Count}");
    }

    #endregion

}

public class Test
{

    #region Methods

    public void GenericHandle()
    {
        var type = typeof(GenericTestClass<int>);
        var typeHandle = type.TypeHandle;

        var methodInfo = type.GetMethod(nameof(GenericTestClass<int>.Generic))!;
        var methodHandle = methodInfo.MethodHandle;
        var m2 = MethodBase.GetMethodFromHandle(methodHandle, typeHandle)!;

        Console.WriteLine("GetGenericMethodFromHandle");
        var array = Attribute.GetCustomAttributes(m2, typeof(MyAttribute), true);
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] is MyAttribute att)
            {
                att.Do();
            }
        }
    }

    public void GetAttributes()
    {
        var method = typeof(TestClass).GetMethod(nameof(TestClass.Method))!;
        var attributes = method.GetCustomAttributes<MyAttribute>(true);

        Console.WriteLine("GetCustomAttributes");
        foreach (var myAttribute in attributes)
        {
            myAttribute.Do();
        }
        //output:
        //MyAttribute 30
        //MyAttribute 40
        //MyAttribute 50
    }

    public void Handle()
    {
        var typeHandle = typeof(TestClass).TypeHandle;

        var methodInfo = typeof(TestClass).GetMethod(nameof(TestClass.Method))!;
        var methodHandle = methodInfo.MethodHandle;
        var m2 = MethodBase.GetMethodFromHandle(methodHandle)!;

        Console.WriteLine("GetMethodFromHandle");
        var array = Attribute.GetCustomAttributes(m2, typeof(MyAttribute), true);
        for (var i = 0; i < array.Length; i++)
        {
            if (array[i] is MyAttribute att)
            {
                att.Do();
            }
        }
    }

    public void ReferenceGenericHandle()
    {
        var mA = typeof(GenericTestClass<A>).GetMethod(nameof(GenericTestClass<A>.Generic))!;
        var mB = typeof(GenericTestClass<B>).GetMethod(nameof(GenericTestClass<B>.Generic))!;

        Console.WriteLine($"AGenericMethodHandle {mA.MethodHandle.Value}");
        Console.WriteLine($"BGenericMethodHandle {mB.MethodHandle.Value}");
        //mA.MethodHandle.Value == mB.MethodHandle.Value
        //generic arguments is reference type, the Method has same MethodHandle
    }

    #endregion

    [My(10)]
    private sealed class TestClass : Base
    {

        #region Properties

        [My(20)] public string Prop { get; set; } = null!;

        #endregion

        #region Methods

        [My(30)]
        [My(40)]
        public override void Method()
        {
            //todo cuizj: get instance attributes
        }

        #endregion
    }

    private class Base
    {

        #region Methods

        [My(50)]
        public virtual void Method()
        {
        }

        #endregion
    }

    private sealed class GenericTestClass<T>
    {

        #region Methods

        [My(100)]
#pragma warning disable CA1822 // Mark members as static
        public void Generic<TIn>()
#pragma warning restore CA1822 // Mark members as static
        {
            //for test
        }

        #endregion
    }

    private sealed class A
    {
    }

    private sealed class B
    {
    }
}
