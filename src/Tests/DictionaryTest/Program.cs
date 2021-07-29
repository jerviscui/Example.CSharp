using BenchmarkDotNet.Running;

namespace DictionaryTest
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            //new AddTests().OneThread_Test();
            //new AddTests().MultiThreads_Test();

            //var stopwatch = new Stopwatch();
            //var t = new GetOrAddTests();
            //t.AddByLock_Concurrent_Test();
            //t.AddByRepeatCreate_Concurrent_Test();

            //stopwatch.Restart();
            //t.AddByLock_Concurrent_Test();
            //stopwatch.Stop();
            //Print.Microsecond(stopwatch, "dic   :");
            //stopwatch.Restart();
            //t.AddByRepeatCreate_Concurrent_Test();
            //stopwatch.Stop();
            //Print.Microsecond(stopwatch, "conDic:");

            BenchmarkRunner.Run<GetOrAddTests>();
        }
    }
}
