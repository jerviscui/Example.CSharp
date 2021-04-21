﻿using System;
using System.Reflection;

namespace TypeHandleTest
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    public class MyAttribute : Attribute
    {
        public int Count { get; set; }

        public void Do()
        {
            Console.WriteLine($"MyAttribute {Count}");
        }

        /// <inheritdoc />
        public MyAttribute(int count)
        {
            Count = count;
        }
    }

    public class Test
    {
        [My(10)]
        class TestClass : Base
        {
            [My(20)]
            public string Prop { get; set; }

            [My(30)]
            [My(40)]
            public override void Method()
            {
                //todo cuizj: get instance attributes
            }
        }

        class Base
        {
            [My(50)]
            public virtual void Method()
            {
            }
        }

        class GenericTestClass<T>
        {
            [My(100)]
            public void Generic<T>()
            {

            }
        }

        class A
        {
            
        }

        class B
        {

        }

        public void GetAttributes()
        {
            var method = typeof(TestClass).GetMethod(nameof(TestClass.Method))!;
            var attributes = method.GetCustomAttributes<MyAttribute>(true);

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

            var array = Attribute.GetCustomAttributes(m2, typeof(MyAttribute), true);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] is MyAttribute att)
                {
                    att.Do();
                }
            }
        }

        public void GenericHandle()
        {
            var type = typeof(GenericTestClass<int>);
            var typeHandle = type.TypeHandle;

            var methodInfo = type.GetMethod(nameof(GenericTestClass<int>.Generic))!;
            var methodHandle = methodInfo.MethodHandle;
            var m2 = MethodBase.GetMethodFromHandle(methodHandle, typeHandle)!;
            
            var array = Attribute.GetCustomAttributes(m2, typeof(MyAttribute), true);
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] is MyAttribute att)
                {
                    att.Do();
                }
            }
        }

        public void ReferenceGenericHandle()
        {
            var mA = typeof(GenericTestClass<A>).GetMethod(nameof(GenericTestClass<A>.Generic));
            var mB = typeof(GenericTestClass<B>).GetMethod(nameof(GenericTestClass<B>.Generic));

            Console.WriteLine($"{mA.MethodHandle.Value}");
            Console.WriteLine($"{mB.MethodHandle.Value}");
            //mA.MethodHandle.Value == mB.MethodHandle.Value
            //generic arguments is reference type, the Method has same MethodHandle
        }
    }
}