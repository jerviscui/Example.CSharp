using System.Diagnostics;
using Common;

namespace AutoPropertiesTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //            Console.WriteLine("Hello World!");

            //            var t = new Test("str", null, 1, null, "untouched");
            //            Console.WriteLine(@$"{nameof(Test.StrProp)}:{t.StrProp}, 
            //{nameof(Test.StrNullProp)}:{t.StrNullProp}, 
            //{nameof(Test.IntProp)}:{t.IntProp}, 
            //{nameof(Test.IntNullProp)}:{t.IntNullProp}, 
            //{nameof(Test.UntouchedProperty)}:{t.UntouchedProperty}");
            //            Console.WriteLine();

            //            try
            //            {
            //                new Child("p", "t", "");
            //            }
            //            catch (ArgumentException e)
            //            {
            //                Console.WriteLine(e);
            //            }

            var watch = new Stopwatch();

            watch.Start();
            var test = new InheritTest("", "");
            watch.Stop();
            Print.Microsecond(watch);

            watch.Restart();
            test.Name = "";
            watch.Stop();
            Print.Microsecond(watch);
            watch.Restart();
            test.Name = "";
            watch.Stop();
            Print.Microsecond(watch);
        }
    }
}
