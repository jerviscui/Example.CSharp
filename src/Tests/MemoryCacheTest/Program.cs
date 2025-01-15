using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace MemoryCacheTest
{
    internal sealed class Program
    {
        private static void Main(string[] args)
        {
            //DependencyTest();

            //UsingScopeTest();

            ChangeTokenTest();
        }

        private static void DependencyTest()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            string inner = "inner";
            string outer = "outer";

            using (var entry = cache.CreateEntry(outer))
            {
                entry.Value = DateTime.Now;
                //entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(10); //会被 inner 覆盖

                cache.Set(inner, DateTime.Now.AddMinutes(1), TimeSpan.FromSeconds(5));
            }

            int i = 0;
            while (true)
            {
                Thread.Sleep(1000);
                Console.WriteLine(cache.Get<DateTime?>(outer));
                Console.WriteLine(cache.Get<DateTime?>(inner));
                Console.WriteLine(i++);
                Console.WriteLine();

                if (!cache.TryGetValue(inner, out DateTime value) && !cache.TryGetValue(outer, out DateTime value2))
                {
                    break;
                }
            }

            Console.WriteLine(cache.Get<DateTime?>(outer));
            Console.WriteLine(cache.Get<DateTime?>(inner));

            //如果 using 不用括号会执行到这里才释放
        }

        private static void UsingScopeTest()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            string key = "inner";

            SetCache(cache, key);

            Console.WriteLine($"Test2 {cache.Get<DateTime?>(key)}");

            void SetCache(IMemoryCache m, string k)
            {
                using var entry = m.CreateEntry(k);
                entry.Value = DateTime.Now;
            }
        }

        public static void GetSetTest()
        {
            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            string key = "key";

            // Look for cache key.
            if (!cache.TryGetValue(key, out DateTime value))
            {
                // Key not in cache, so get data.
                value = DateTime.Now;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(1));

                // Save data in cache.
                cache.Set(key, value, cacheEntryOptions);
            }

            Console.WriteLine(value);

            cache.GetOrCreate(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);

                return DateTime.Now;
            });

            cache.GetOrCreateAsync(key, entry =>
            {
                entry.SlidingExpiration = TimeSpan.FromSeconds(3);
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(1);

                return Task.FromResult(DateTime.Now);
            });
        }

        private static void ChangeTokenTest()
        {
            string inner = "inner";
            string outer = "outer";

            var cts = new CancellationTokenSource();

            IMemoryCache cache = new MemoryCache(new MemoryCacheOptions());

            using (var entry = cache.CreateEntry(outer))
            {
                entry.Value = DateTime.Now;

                cache.Set(inner, DateTime.Now.AddMinutes(1), new CancellationChangeToken(cts.Token));
            }

            Console.WriteLine(cache.Get<DateTime?>(outer));
            Console.WriteLine(cache.Get<DateTime?>(inner));

            cts.Cancel();

            Console.WriteLine("cleared");
            Console.WriteLine(cache.Get<DateTime?>(outer));
            Console.WriteLine(cache.Get<DateTime?>(inner));

            //outer & inner cleared both
        }
    }
}
