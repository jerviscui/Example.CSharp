using System;

namespace CtorTest
{
    internal class ReflectionTest
    {
        private class MyClass
        {
            protected MyClass()
            {
                Console.WriteLine("protected");
            }
        }

        public static void CreateInstance_UseProtectedCtor_Test()
        {
            Activator.CreateInstance(typeof(MyClass), true);
        }
    }
}
