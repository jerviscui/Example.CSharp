using System.Diagnostics;
using BenchmarkDotNet.Running;
using Common;

namespace DictionaryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var stopwatch = new Stopwatch();
            //var t = new GetOrAddTests();
            //t.AddByLock_Concurrent_Test();
            //t.AddByRepeatCreate_Concurrent_Test();

            //stopwatch.Restart();
            //t.AddByLock_Concurrent_Test();
            //stopwatch.Stop();
            //Print.Microsecond(stopwatch);
            //stopwatch.Restart();
            //t.AddByRepeatCreate_Concurrent_Test();
            //stopwatch.Stop();
            //Print.Microsecond(stopwatch);

            BenchmarkRunner.Run<GetOrAddTests>();
        }
    }
}
