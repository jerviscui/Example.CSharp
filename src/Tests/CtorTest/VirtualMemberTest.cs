using System;

namespace CtorTest
{
    internal class VirtualMemberTest
    {
        private class Base
        {
            protected Base()
            {
                Console.WriteLine("Base 构造函数");

                //virtual member call in constructor
                Console.WriteLine($"Base: {S}");
            }

            public virtual string S { get; set; } = "Base's S";
        }

        private class Derived : Base
        {
            public Derived()
            {
                Console.WriteLine("Derived 构造函数");

                //virtual member call in constructor
                Console.WriteLine($"Derived: {S}");
            }

            /// <inheritdoc />
            public override string S { get; set; } = "Derived's S";
        }

        public static void BaseCtor_UseOverrideMember_Test()
        {
            _ = new Derived();

            //Base 构造函数
            //Base: Derived's S
            //Derived 构造函数
            //Derived: Derived's S
        }
    }
}
