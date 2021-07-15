using System;

namespace CtorTest
{
    /// <summary>
    /// 初始化和基类构造函数的执行顺序
    /// <para>https://docs.microsoft.com/en-us/archive/blogs/ericlippert/why-do-initializers-run-in-the-opposite-order-as-constructors-part-one</para>
    /// </summary>
    public class InitializerTest
    {
        public void Assignment_ExecutionOrder()
        {
            new AssignmentOrder() { S = "third" };
        }


        public void Initializer_Ctor_ExecutionOrder()
        {
            new Derived() { S = "1" };

            //Foo 构造函数：Derived initializer
            //Foo 构造函数：Base initializer
            //Base 构造函数
            //Derived 构造函数
        }
    }

    class AssignmentOrder
    {
        public string S { get; set; } = "first";

        public AssignmentOrder()
        {
            S = "second";
        }
    }

    class Foo
    {
        public Foo(string s)
        {
            Console.WriteLine("Foo 构造函数：{0}", s);
        }

        public void Bar() { }
    }

    class Base
    {
        readonly Foo baseFoo = new Foo("Base initializer");

        public virtual string S { get; set; } = "Base's S";

        public Base()
        {
            Console.WriteLine("Base 构造函数");

            //virtual member call in constructor
            Console.WriteLine(S);
        }
    }

    class Derived : Base
    {
        readonly Foo derivedFoo = new Foo("Derived initializer");

        /// <inheritdoc />
        public override string S { get; set; } = "Derived's S";

        public Derived()
        {
            Console.WriteLine("Derived 构造函数");

            //virtual member call in constructor
            Console.WriteLine(S);
        }
    }
}
