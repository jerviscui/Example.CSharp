using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;

namespace TypeHandleTest
{
    [GroupBenchmarksBy(BenchmarkLogicalGroupRule.ByCategory)]
    [CategoriesColumn]
    [MemoryDiagnoser]
    public class BenchmarkTests
    {
        class Test
        {
            public int Method()
            {
                int i = 0;
                int j = i++;

                return j;
            }
        }

        public Dictionary<string, MethodInfo> MethodInfos { get; set; } = new();

        public Dictionary<string, RuntimeMethodHandle> MethodHandles { get; set; } = new();

        public static string Key;

        static BenchmarkTests()
        {
            var method = typeof(Test).GetMethod(nameof(Test.Method))!;
            Key = $"{method.ReflectedType}+{method.Name}";
        }

        public BenchmarkTests()
        {
            var method = typeof(Test).GetMethod(nameof(Test.Method))!;

            MethodInfos.Add(Key, method);
            for (int i = 1; i < 1000; i++)
            {
                MethodInfos.Add(i.ToString(), method);
            }

            var handle = method.MethodHandle;
            MethodHandles.Add(Key, handle);
            for (int i = 1; i < 1000; i++)
            {
                MethodHandles.Add(i.ToString(), handle);
            }
        }

        [BenchmarkCategory("ExecTime")]
        [Benchmark(Description = "MethodInfoTest", Baseline = true)]
        public MethodInfo? MethodInfoTest()
        {
            return MethodInfos.GetValueOrDefault(Key);
        }

        [BenchmarkCategory("ExecTime")]
        [Benchmark(Description = "MethodHandleTest")]
        public MethodBase? MethodHandleTest()
        {
            var hand = MethodHandles.GetValueOrDefault(Key);

            return MethodBase.GetMethodFromHandle(hand);
        }
    }
}
