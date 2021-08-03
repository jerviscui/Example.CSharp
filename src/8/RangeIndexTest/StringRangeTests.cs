using System.Diagnostics;
using Common;

namespace RangeIndexTest
{
    internal class StringRangeTests
    {
        private const string Content = "1234567890";

        private static void SubString_Test() => _ = Content.Substring(1);

        private static void SubString_ByIndex_Test() => _ = Content[1..];

        public static void RunTest()
        {
            var i = 1_000_000;
            var stopwatch = new Stopwatch();

            stopwatch.Start();
            for (var j = 0; j < i; j++)
            {
                //SubString_ByIndex_Test();
                SubString_Test();
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch);

            stopwatch.Restart();
            for (var j = 0; j < i; j++)
            {
                //SubString_Test();
                SubString_ByIndex_Test();
            }
            stopwatch.Stop();
            Print.Microsecond(stopwatch);

            //使用 Index 后 18% 左右的性能损失
            //11,429 us
            //13,726 us
        }
    }
}
