using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace ForeachTest
{
    [MemoryDiagnoser]
    public class ToArrayAndListTests
    {
        //|    Method |   Count |          Mean |       Error |      StdDev |        Median | Ratio | RatioSD |    Gen 0 |    Gen 1 |    Gen 2 | Allocated |
        //|---------- |-------- |--------------:|------------:|------------:|--------------:|------:|--------:|---------:|---------:|---------:|----------:|
        //| ToArray() |    5000 |      3.331 us |   0.0583 us |   0.0716 us |      3.335 us |  1.00 |    0.00 |   4.7836 |        - |        - |     20 KB |
        //|  ToList() |    5000 |     11.902 us |   0.1116 us |   0.0989 us |     11.864 us |  3.57 |    0.10 |   4.7760 |        - |        - |     20 KB |
        //|           |         |               |             |             |               |       |         |          |          |          |           |
        //| ToArray() | 5000000 |  7,936.091 us | 158.2916 us | 433.3207 us |  8,058.860 us |  1.00 |    0.00 | 156.2500 | 156.2500 | 156.2500 | 19,531 KB |
        //|  ToList() | 5000000 | 15,401.032 us | 303.9784 us | 405.8025 us | 15,384.870 us |  1.94 |    0.13 | 281.2500 | 281.2500 | 281.2500 | 19,531 KB |

        [Params(5_000, 5_000_000)]
        public int Count { get; set; }

        public IEnumerable<int> Items => Enumerable.Range(0, Count);

        [Benchmark(Description = "ToArray()", Baseline = true)]
        public int[] ToArray() => Items.ToArray();

        [Benchmark(Description = "ToList()")]
        public List<int> ToList() => Items.ToList();
    }
}
