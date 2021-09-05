using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace ForeachTest
{
    [MemoryDiagnoser]
    public class ForeachTest
    {
        //|                         Method |      Mean |     Error |    StdDev | Ratio | RatioSD |   Gen 0 |   Gen 1 |   Gen 2 |    Allocated |
        //|------------------------------- |----------:|----------:|----------:|------:|--------:|--------:|--------:|--------:|-------------:|
        //|               ArrayForeachTest |  1.270 ms | 0.0032 ms | 0.0025 ms |  1.00 |    0.00 |       - |       - |       - |            - |
        //|         IEnumerableForeachTest | 19.236 ms | 0.1352 ms | 0.1129 ms | 15.16 |    0.08 |       - |       - |       - |         32 B |
        //|               IListForeachTest | 30.875 ms | 0.3186 ms | 0.2660 ms | 24.33 |    0.21 |       - |       - |       - |         40 B |
        //|                ListForeachTest | 10.089 ms | 0.0644 ms | 0.0571 ms |  7.94 |    0.05 |       - |       - |       - |            - |
        //|       IReadOnlyListForeachTest | 30.732 ms | 0.2373 ms | 0.2103 ms | 24.17 |    0.18 |       - |       - |       - |         40 B |
        //|        ReadOnlyListForeachTest | 30.976 ms | 0.4140 ms | 0.3670 ms | 24.41 |    0.31 |       - |       - |       - |         40 B |
        //| ReadOnlyListToArrayForeachTest |  7.917 ms | 0.0771 ms | 0.0721 ms |  6.25 |    0.06 | 93.7500 | 93.7500 | 93.7500 | 20,000,023 B |

        public IEnumerable<int> IEnumerable = Enumerable.Range(0, 5_000_000).ToArray();

        public int[] Array = Enumerable.Range(0, 5_000_000).ToArray();

        public IList<int> IList = Enumerable.Range(0, 5_000_000).ToList();

        public List<int> List = Enumerable.Range(0, 5_000_000).ToList();

        public ReadOnlyCollection<int> ReadOnlyList = Enumerable.Range(0, 5_000_000).ToList().AsReadOnly();

        public IReadOnlyCollection<int> IReadOnlyList = Enumerable.Range(0, 5_000_000).ToList().AsReadOnly();

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

        [Benchmark(Description = "IReadOnlyListForeachTest")]
        public void IReadOnlyListForeachTest()
        {
            foreach (var item in IReadOnlyList)
            {
                var a = item + 1;
            }
        }

        [Benchmark(Description = "ReadOnlyListForeachTest")]
        public void ReadOnlyListForeachTest()
        {
            foreach (var item in ReadOnlyList)
            {
                var a = item + 1;
            }
        }

        [Benchmark(Description = "ReadOnlyListToArrayForeachTest")]
        public void ReadOnlyListToArrayForeachTest()
        {
            foreach (var item in ReadOnlyList.ToArray())
            {
                var a = item + 1;
            }
        }
    }
}
