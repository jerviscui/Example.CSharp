using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace ForeachTest
{
    [MemoryDiagnoser]
    public class ToArrayAndListTests
    {
        [Params(5_000, 5_000_000)]
        public int Count { get; set; }

        public IEnumerable<int> Items => Enumerable.Range(0, Count);

        [Benchmark(Description = "ToArray()", Baseline = true)]
        public int[] ToArray() => Items.ToArray();

        [Benchmark(Description = "ToList()")]
        public List<int> ToList() => Items.ToList();
    }

    [MemoryDiagnoser]
    public class ForeachTest
    {
        public IEnumerable<int> IEnumerable = Enumerable.Range(0, 5_000_000).ToArray();

        public int[] Array = Enumerable.Range(0, 5_000_000).ToArray();

        public IList<int> IList = Enumerable.Range(0, 5_000_000).ToArray();

        public List<int> List = Enumerable.Range(0, 5_000_000).ToArray().ToList();

        [Benchmark(Description = "ArrayForeachTest", Baseline = true)]
        public void ArrayForeachTest()
        {
            foreach (var item in Array)
            {
                var a = item + 1;
            }
        }

        [Benchmark(Description = "IEnumerableForeachTest")]
        public void IEnumerableForeachTest()
        {
            foreach (var item in IEnumerable)
            {
                var a = item + 1;
            }
        }

        [Benchmark(Description = "IListForeachTest")]
        public void IListForeachTest()
        {
            foreach (var item in IList)
            {
                var a = item + 1;
            }
        }

        [Benchmark(Description = "ListForeachTest")]
        public void ListForeachTest()
        {
            foreach (var item in List)
            {
                var a = item + 1;
            }
        }
    }
}
