using System;
using BenchmarkDotNet.Attributes;
using Common;

namespace DelegateTest
{
    [MemoryDiagnoser]
    public class BenchmarkTest
    {
        [Benchmark]
        public void GetMethodTest()
        {
            var a = new DelegateTest.A();
            var method = typeof(DelegateTest.A).GetMethod("Test")!;
            method.Invoke(a, Array.Empty<object>());
        }

        [Benchmark]
        public void NewDelegateTest()
        {
            var a = new DelegateTest.A();
            var method = a.GetAction();
            method();
        }

        [Benchmark]
        public void CacheDelegateTest()
        {
            var a = new DelegateTest.A();
            var method = a.GetActionCache();
            method();
        }
    }
}