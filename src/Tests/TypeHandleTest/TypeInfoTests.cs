using System;
using System.Reflection;

namespace TypeHandleTest
{
    public class TypeInfoTests
    {
        class A
        {
            public int Prop1 { get; set; }
            int PrivateProp1 { get; set; }
        }

        class B : A
        {
            public int Prop2 { get; set; }
            int PrivateProp2 { get; set; }
        }

        public void Type_GetProperties_Test()
        {
            foreach (var pi in typeof(B).GetProperties())
            {
                Console.WriteLine(pi.Name);
            }

            //Prop2
            //Prop1
        }

        public void Type_GetRuntimeProperties_Test()
        {
            foreach (var pi in typeof(B).GetRuntimeProperties())
            {
                Console.WriteLine(pi.Name);
            }

            //Prop2
            //PrivateProp2
            //Prop1
        }

        public void TypeInfo_DeclaredProperties_Test()
        {
            foreach (var pi in typeof(B).GetTypeInfo().DeclaredProperties)
            {
                Console.WriteLine(pi.Name);
            }

            //Prop2
            //PrivateProp2
        }

    }
}
