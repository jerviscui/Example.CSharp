using System;
using System.Text;
using StackExchange.Redis;

namespace StackExchangeRedisTest
{
    internal class HashTest
    {
        public static void HashSet_Test()
        {
            var database = DatabaseProvider.GetDatabase();
            var key = "hashkey";

            database.HashSet(key, "field1", "value1");
            database.HashSet(key, "field2", "value2");
        }

        public static void HashSet_WithHashEntry_Test()
        {
            var database = DatabaseProvider.GetDatabase();
            var key = "hashkey2";

            database.HashSet(key, new HashEntry[] { new("field1", "value1"), new("field2", "value2") });
        }

        private static void Set(string key)
        {
            var database = DatabaseProvider.GetDatabase();

            database.HashSet(key, "field1", DateTime.Now.ToString("O"));
            database.HashSet(key, "field2", DateTime.Now.ToString("O"));
        }

        public static void HashGet_Test()
        {
            var key = "hashkey3";

            Set(key);

            var database = DatabaseProvider.GetDatabase();

            var value1 = database.HashGet(key, "field1");
            Console.WriteLine(value1);
        }

        public static void HashGetAll_Test()
        {
            var key = "hashkey4";

            Set(key);

            var database = DatabaseProvider.GetDatabase();

            var values = database.HashGetAll(key);

            foreach (var t in values)
            {
                Console.WriteLine($"{t.Name}:{t.Value}");
            }
        }

        public static void HashGetLease_Test()
        {
            var key = "hashkey5";

            Set(key);

            var database = DatabaseProvider.GetDatabase();
            var bytes = database.HashGetLease(key, "field1");

            var time = DateTime.Parse(Encoding.UTF8.GetString(bytes.Span));
            Console.WriteLine(time.ToString("O"));
        }
    }
}
