using System;

namespace CtorTest
{
    /// <summary>
    /// 初始化和基类构造函数的执行顺序
    /// <para>https://docs.microsoft.com/en-us/archive/blogs/ericlippert/why-do-initializers-run-in-the-opposite-order-as-constructors-part-one</para>
    /// </summary>
    public class InitializerTest
    {
        public static void Assignment_ExecutionOrder()
        {
            _ = new AssignmentOrder { S = "third" };
        }

        public static void Initializer_Ctor_ExecutionOrder()
        {
            _ = new Derived { S = "1" };

            //Foo 构造函数：Derived initializer
            //Foo 构造函数：Base initializer
            //Base 构造函数
            //Derived 构造函数
        }
    }

    internal class AssignmentOrder
    {
        public AssignmentOrder()
        {
            S = "second";
        }

        public string S { get; set; } = "first";
    }

    internal class Foo
    {
        public Foo(string s)
        {
            Console.WriteLine("Foo 构造函数：{0}", s);
        }
    }

    internal class Base
    {
#pragma warning disable IDE0052 // 删除未读的私有成员
        private readonly Foo _baseFoo = new("Base initializer");
#pragma warning restore IDE0052 // 删除未读的私有成员

        public Base()
        {
            Console.WriteLine("Base 构造函数");

            //virtual member call in constructor
            Console.WriteLine(S);
        }

        public virtual string S { get; set; } = "Base's S";
    }

    internal class Derived : Base
    {
#pragma warning disable IDE0052 // 删除未读的私有成员
        private readonly Foo _derivedFoo = new("Derived initializer");
#pragma warning restore IDE0052 // 删除未读的私有成员

        public Derived()
        {
            Console.WriteLine("Derived 构造函数");

            //virtual member call in constructor
            Console.WriteLine(S);
        }

        /// <inheritdoc />
        public override string S { get; set; } = "Derived's S";
    }
}
