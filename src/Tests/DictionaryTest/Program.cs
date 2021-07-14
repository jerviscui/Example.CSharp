using BenchmarkDotNet.Running;

namespace DictionaryTest
{
    class Program
    {
        static void Main(string[] args)
        {
            ////ToArray 快
            //BenchmarkRunner.Run<ToArrayAndListTests>();

            ////array 最快，IEnumerable IList 有装箱，
            //BenchmarkRunner.Run<ForeachTest>();
            
            ////Test();
        }
    }
}
