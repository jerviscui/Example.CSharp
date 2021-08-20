using System;

namespace CtorTest
{
    /// <summary>
    /// 初始化和基类构造函数的执行顺序
    /// <para>https://docs.microsoft.com/en-us/archive/blogs/ericlippert/why-do-initializers-run-in-the-opposite-order-as-constructors-part-one</para>
    /// </summary>
    public class InitializerTest
    {
        private class AssignmentOrder
        {
            public AssignmentOrder()
            {
                S = "second";
            }

            public string S { get; set; } = "first";
        }

        public static void Assignment_ExecutionOrder()
        {
            _ = new AssignmentOrder { S = "third" };

            //1. initializer "first"
            //2. constructor "second"
            //3. assignment "third"
        }

        private class BaseCtorTest
        {
            protected BaseCtorTest()
            {
                Console.WriteLine(nameof(BaseCtorTest));
            }
        }

        private class DeriveCtorTest : BaseCtorTest
        {
            public DeriveCtorTest()
            {
                Console.WriteLine(nameof(DeriveCtorTest));
            }
        }

        public static void Ctor_ExecutionOrder()
        {
            _ = new DeriveCtorTest();

            //1. base class ctor "BaseCtorTest"
            //2. derive class ctor "DeriveCtorTest"
        }

        public static void Ctor_Initializer_ExecutionOrder()
        {
            _ = new Derived();

            //1. derive class initializer   "Foo 构造函数：Derived initializer"
            //2. base class initializer     "Foo 构造函数：Base initializer"
            //1. base class ctor            "Base 构造函数"
            //2. derive class ctor          "Derived 构造函数"

            //Foo 构造函数：Derived initializer
            //Foo 构造函数：Base initializer
            //Base 构造函数
            //Derived 构造函数
        }

        private class Foo
        {
            public Foo(string s)
            {
                Console.WriteLine("Foo 构造函数：{0}", s);
            }
        }

        private class Base
        {
#pragma warning disable IDE0052 // 删除未读的私有成员
            private readonly Foo _baseFoo = new("Base initializer");
#pragma warning restore IDE0052 // 删除未读的私有成员

            protected Base()
            {
                Console.WriteLine("Base 构造函数");
            }
        }

        private class Derived : Base
        {
#pragma warning disable IDE0052 // 删除未读的私有成员
            private readonly Foo _derivedFoo = new("Derived initializer");
#pragma warning restore IDE0052 // 删除未读的私有成员

            public Derived()
            {
                Console.WriteLine("Derived 构造函数");
            }
        }
    }
}
