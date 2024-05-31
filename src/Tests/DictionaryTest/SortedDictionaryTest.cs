using System.Collections.Generic;
using System.Linq;
using BenchmarkDotNet.Attributes;

namespace DictionaryTest
{
    [MemoryDiagnoser]
    public class SortedDictionaryTest
    {
        // SortedDictionary 要慢 25 倍！
        //|                 Method |       Mean |    Error |   StdDev |     Gen 0 |     Gen 1 |     Gen 2 | Allocated |
        //|----------------------- |-----------:|---------:|---------:|----------:|----------:|----------:|----------:|
        //| AddTo_SortedDictionary | 2,555.3 ms | 41.63 ms | 38.94 ms | 4000.0000 | 2000.0000 |         - |     53 MB |
        //|       AddTo_Dictionary |   116.8 ms |  2.33 ms |  4.20 ms | 1000.0000 | 1000.0000 | 1000.0000 |     72 MB |

        private static readonly string[] StrArray = Enumerable.Range(1, 1_000_000).Select(o => o.ToString()).ToArray();

        [Benchmark]
#pragma warning disable CA1822 // Mark members as static
        public void AddTo_SortedDictionary()
#pragma warning restore CA1822 // Mark members as static
        {
            var sortedDictionary = new SortedDictionary<string, string>();

            for (int i = 0; i < StrArray.Length; i++)
            {
                sortedDictionary.Add(StrArray[i], StrArray[i]);
            }
        }

        [Benchmark]
#pragma warning disable CA1822 // Mark members as static
        public void AddTo_Dictionary()
#pragma warning restore CA1822 // Mark members as static
        {
            var dictionary = new Dictionary<string, string>();

            for (int i = 0; i < StrArray.Length; i++)
            {
                dictionary.Add(StrArray[i], StrArray[i]);
            }
        }
    }
}
