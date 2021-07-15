using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DictionaryTest
{
    [MemoryDiagnoser]
    public class GetOrAddTests
    {
        private readonly Dictionary<string, string> _dic = new();
        private readonly ConcurrentDictionary<string, string> _dicConcurrent = new();

        private readonly object _lock = new();

        [Benchmark]
        public void AddByRepeatCreate_OneThread_Test()
        {
            var key = "repeat";

            for (int i = 0; i < 1_000_000; i++)
            {
                GetOrAddByTpl(key);
            }
        }

        [Benchmark]
        public void AddByLock_OneThread_Test()
        {
            var key = "lock";

            for (int i = 0; i < 1_000_000; i++)
            {
                GetOrAddByLock(key);
            }
        }

        [Benchmark]
        public void AddByRepeatCreate_Concurrent_Test()
        {
            var s = "repeat";

            var tasks = new Task[1_000];
            for (int i = 0; i < tasks.Length; i++)
            {
                var key = s;
                if (i % 10 == 0)
                {
                    s = $"{s}{(i / 10).ToString()}";
                }
                tasks[i] = new Task(() => GetOrAddByTpl(key));
            }

            Parallel.ForEach(tasks, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, task => task.Start());

            Task.WaitAll(tasks);
        }

        [Benchmark]
        public void AddByLock_Concurrent_Test()
        {
            var s = "lock";

            var tasks = new Task[1_000];
            for (int i = 0; i < tasks.Length; i++)
            {
                var key = s;
                if (i % 10 == 0)
                {
                    s = $"{s}{(i / 10).ToString()}";
                }
                tasks[i] = new Task(() => GetOrAddByLock(key));
            }

            Parallel.ForEach(tasks, new ParallelOptions() { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 }, task => task.Start());

            Task.WaitAll(tasks);
        }

        private string GetOrAddByTpl(string key)
        {
            if (!_dicConcurrent.TryGetValue(key, out string? value))
            {
                value = CreateValue();
                value = _dicConcurrent.GetOrAdd(key, value);
            }

            return value;
        }

        private string GetOrAddByLock(string key)
        {
            if (!_dic.TryGetValue(key, out string? value))
            {
                lock (_lock)
                {
                    if (!_dic.TryGetValue(key, out value))
                    {
                        value = CreateValue();
                        _dic.Add(key, value);
                    }
                }
            }

            return value;
        }

        private static string CreateValue() => DateTime.Now.ToString("s");
    }
}
