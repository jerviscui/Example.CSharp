using System;
using System.Diagnostics;
using Common;

namespace InfoOfTest
{
    class Program
    {
        static void Main(string[] args)
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
        private string _f;

        public string Name { get; set; }

        public TOut? TestMethod<TIn, TOut>(TIn input)
        {
            return default(TOut);
        }
    }
}
