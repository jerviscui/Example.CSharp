using System;
using System.Linq;
using System.Text.Json;
using CacheManager.Core;
using CacheManager.MicrosoftCachingMemory;
using CacheManager.Redis;
using Microsoft.Extensions.Logging;

namespace CacheManagerTest
{
    internal sealed class Program
    {
        private const string ConfigKey = "redis";

        private static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                MultiClientTest(args[0]);
            }

            using (var manager = GetCacheManager<string>())
            {
                var value = manager.GetOrAdd("test", "region", (s, r) => "value");
                Console.WriteLine(value);
            }

            SerializeTest();

            EventTest();

            Console.ReadKey();

            //todo cuizj: how to direct use of StackExchange.Redis ;
            //var handle = manager2.CacheHandles.First() as RedisCacheHandle<Student>;
            //ConnectionMultiplexer a = null!;
            //a.GetDatabase(1).HashGetAsync("k", "f", CommandFlags.DemandSlave);

            //todo cuizj: use hash.set
        }

        private static void SerializeTest()
        {
            using var manager2 = GetCacheManager<Student>();

            var s = manager2.GetOrAdd("test", "students",
                new Student { Id = 1, FirstMidName = "F", LastName = "X", EnrollmentDate = DateTime.Now });
            Console.WriteLine(JsonSerializer.Serialize(s));

            //Console.WriteLine(manager2.Get<string>("test", "students")); //todo cuizj: 如何直接取 string，不反序列化
        }

        private static void EventTest()
        {
            using var manager2 = GetCacheManager<Student>();

            manager2.OnRemove += (sender, eventArgs) =>
                Console.WriteLine($"{sender} {eventArgs.Region}:{eventArgs.Key} removed."); //通过 Remove() 时触发，全局触发一次

            manager2.Add("s2", new Student { Id = 2 });
            manager2.Remove("s2"); //OnRemove
            //:s2 removed.

            manager2.OnRemoveByHandle += (sender, args) =>
                Console.WriteLine($"{sender} {args.Level}:{args.Region}:{args.Key} removed."); //缓存过期时触发，每个缓存层触发一次,
            //可与 .EnableKeyspaceEvents() 结合使用                                                                                              
            manager2.Add(new CacheItem<Student>("s3", new Student { Id = 2 },
                ExpirationMode.Sliding, TimeSpan.FromSeconds(10))); //OnRemoveByHandle
        }

        private static ICacheManager<TValue> GetCacheManager<TValue>()
        {
            var configuration = new ConfigurationBuilder()
                .WithJsonSerializer() //todo cuizj: 使用 System.Text.Json.JsonSerializer
                .WithMicrosoftMemoryCacheHandle("memory")
                .And
                .WithRedisConfiguration(ConfigKey, builder =>
                    builder.WithAllowAdmin().WithDatabase(0).WithEndpoint("127.0.0.1", 7000)
                        .EnableKeyspaceEvents()) //需要开启 redis notify-keyspace-events "Exe"
                .WithMaxRetries(50)
                .WithRetryTimeout(50)
                .WithRedisBackplane(ConfigKey) //启用二级缓存同步
                .WithRedisCacheHandle(ConfigKey)
                .And
                .WithMicrosoftLogging(LoggerFactory.Create(builder =>
                    builder.AddSimpleConsole().SetMinimumLevel(LogLevel.Information)))
                .Build();

            var cacheManager = CacheFactory.FromConfiguration<TValue>(configuration);

            return cacheManager;
        }

        private class Student
        {
            public int Id { get; set; }

            public string? LastName { get; set; } = string.Empty;

            public string FirstMidName { get; set; } = string.Empty;

            public DateTime? EnrollmentDate { get; set; }
        }

        #region MultiClientTest

        private static void MultiClientTest(string client)
        {
            Action<ICacheManager<Student>>? fun = client switch
            {
                "1" => WriteClient,
                "2" => ReadClient,
                "3" => RemoveClient,
                _ => null
            };

            if (fun is not null)
            {
                using var manager = GetCacheManager<Student>();
                manager.OnUpdate += (sender, eventArgs) =>
                    Console.WriteLine($"{sender} {eventArgs.Origin} {eventArgs.Region}:{eventArgs.Key} updated.");

                manager.OnRemove += (sender, eventArgs) =>
                    Console.WriteLine($"{sender} {eventArgs.Origin} {eventArgs.Region}:{eventArgs.Key} removed.");

                manager.OnRemoveByHandle += (sender, args) =>
                    Console.WriteLine($"{sender} {args.Level} {args.Reason} {args.Region}:{args.Key} handle removed.");

                fun(manager);
            }
        }

        private static void WriteClient(ICacheManager<Student> manager)
        {
            var s = manager.GetOrAdd("test", "students",
                new Student { Id = 1, FirstMidName = "F", LastName = "X", EnrollmentDate = DateTime.Now });

            while (true)
            {
                Console.ReadKey();

                manager.AddOrUpdate("test", "students", s, student =>
                {
                    student.EnrollmentDate = DateTime.Now;
                    return student;
                });
            }
        }

        private static void ReadClient(ICacheManager<Student> manager)
        {
            while (true)
            {
                var s = manager.Get("test", "students");
                Console.WriteLine(JsonSerializer.Serialize(s));

                Console.ReadKey();
            }
        }

        private static void RemoveClient(ICacheManager<Student> manager)
        {
            int i = 0;

            while (true)
            {
                Console.WriteLine("1. memory    2. redis    3. manager");
                var key = Console.ReadKey();

                if (key.Key == ConsoleKey.D1 || key.Key == ConsoleKey.NumPad1)
                {
                    Console.WriteLine("remove memory");

                    var handle =
                        manager.CacheHandles.First(o => o.Configuration.Name == "memory") as MemoryCacheHandle<Student>;
                    Console.WriteLine(JsonSerializer.Serialize(manager.Get("test", "students")));
                    handle!.Remove("test", "students");
                    Console.WriteLine(JsonSerializer.Serialize(handle!.Get("test", "students")));
                }
                else if (key.Key == ConsoleKey.D2 || key.Key == ConsoleKey.NumPad2)
                {
                    Console.WriteLine("remove redis");

                    var handle =
                        manager.CacheHandles.First(o => o.Configuration.Name == ConfigKey) as RedisCacheHandle<Student>;
                    handle!.Remove("test", "students");
                }
                else if (key.Key == ConsoleKey.D3 || key.Key == ConsoleKey.NumPad3)
                {
                    Console.WriteLine("remove manager");

                    manager.Remove("test", "students");
                }
                else
                {
                    Console.WriteLine("nothing.");
                }

                i++;
            }
        }

        #endregion
    }
}
