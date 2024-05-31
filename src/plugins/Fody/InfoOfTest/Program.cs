using System.Diagnostics;
using Common;

namespace InfoOfTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var t = typeof(Test);

            var tt = Info.OfType("InfoOfTest", "InfoOfTest.Test");
            var f = Info.OfField<Test>("_f");
            var pg = Info.OfPropertyGet<Test>("Name");
            var m = Info.OfMethod<Test>("TestMethod");

            var watch = new Stopwatch();
            watch.Start();
            for (int i = 0; i < 100; i++)
            {
                typeof(Test).GetField("_f");
            }
            watch.Stop();
            Print.Microsecond(watch, "GetField    :");

            watch.Start();
            for (int i = 0; i < 100; i++)
            {
                Info.OfField<Test>("_f");
            }
            watch.Stop();
            Print.Microsecond(watch, "Info.OfField:");

            //GetField    :      1,549 us
            //Info.OfField:      1,570 us
        }
    }

    public class Test
    {
        private string _f = null!;

        public string Name { get; set; } = null!;

#pragma warning disable CA1822 // Mark members as static
        public TOut? TestMethod<TIn, TOut>(TIn input)
#pragma warning restore CA1822 // Mark members as static
        {
            return default(TOut);
        }
    }
}
