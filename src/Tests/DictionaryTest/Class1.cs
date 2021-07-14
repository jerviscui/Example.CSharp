using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Diagnostics.Runtime.Interop;

namespace DictionaryTest
{
    [MemoryDiagnoser]
    public class Class1
    {
        private readonly Dictionary<string, string> _dic = new();

        private readonly object _lock = new();
        
        //[Benchmark]
        //public void Test()
        //{
        //    var tasks = new Task<ServiceA>[100];
        //    for (int i = 0; i < tasks.Length; i++)
        //    {
        //        tasks[i] = new Task<ServiceA>(() => t.ServiceA);
        //    }

        //    Parallel.ForEach(tasks, new ParallelOptions() { MaxDegreeOfParallelism = 50 }, task => task.Start());

        //    Task.WaitAll(tasks);
        //}

        //public void 
    }
}
