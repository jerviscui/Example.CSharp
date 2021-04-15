using System;

namespace AutoPropertiesTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var t = new Test("str", null, 1, null, "untouched");
            Console.WriteLine(@$"{nameof(Test.StrProp)}:{t.StrProp}, 
{nameof(Test.StrNullProp)}:{t.StrNullProp}, 
{nameof(Test.IntProp)}:{t.IntProp}, 
{nameof(Test.IntNullProp)}:{t.IntNullProp}, 
{nameof(Test.UntouchedProperty)}:{t.UntouchedProperty}");
            Console.WriteLine();

            try
            {
                new Child("p", "t", "");
            }
            catch (ArgumentException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
