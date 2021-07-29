using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NullableEnableTest
{
    public class Item
    {
        public string Name { get; set; }
    }

    public class DictionaryTest
    {
        private readonly ConcurrentDictionary<string, Item[]> _caches = new();

        public void Test()
        {
            //dependency by .Net 5 or standard 2.1
            var nullarray = _caches.GetValueOrDefault("test");
            var emptyarray = _caches.GetValueOrDefault("test2", new Item[0]);
        }
    }
}
