using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace NullableEnableTest
{
    public class Item
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Name { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }

    public class DictionaryTest
    {
        private readonly ConcurrentDictionary<string, Item[]> _caches = new();

        public void Test()
        {
            //dependency by .Net 5 or standard 2.1
            var nullarray = _caches.GetValueOrDefault("test");
            var emptyarray = _caches.GetValueOrDefault("test2", Array.Empty<Item>());
        }
    }
}
