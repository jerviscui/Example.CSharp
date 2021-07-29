using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Volo.Abp.Threading
{
    //singleton
    [Serializable]
    public class AmbientDataContextAmbientScopeProvider<T>
    {
        public static AmbientDataContextAmbientScopeProvider<T> Instance { get; set; } =
            new(new AsyncLocalAmbientDataContext());

        private static readonly ConcurrentDictionary<string, ScopeItem> ScopeDictionary = new();

        private readonly AsyncLocalAmbientDataContext _dataContext;

        private AmbientDataContextAmbientScopeProvider([NotNull] AsyncLocalAmbientDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public T? GetValue(string contextKey)
        {
            var item = GetCurrentItem(contextKey);
            if (item == null)
            {
                return default;
            }

            return item.Value;
        }

        public IDisposable BeginScope(string contextKey, T value)
        {
            var item = new ScopeItem(value, GetCurrentItem(contextKey));

            if (!ScopeDictionary.TryAdd(item.Id, item))
            {
                throw new Exception("Can not add item! ScopeDictionary.TryAdd returns false!");
            }

            _dataContext.SetData(contextKey, item.Id);

            return new DisposeAction(() =>
            {
                ScopeDictionary.TryRemove(item.Id, out item!);

                if (item.Outer == null)
                {
                    _dataContext.SetData(contextKey, null);
                    return;
                }

                _dataContext.SetData(contextKey, item.Outer.Id);
            });
        }

        private ScopeItem? GetCurrentItem(string contextKey)
        {
            return _dataContext.GetData(contextKey) is string itemId ? ScopeDictionary.GetOrDefault(itemId) : null;
        }

        private class ScopeItem
        {
            public string Id { get; }

            public ScopeItem? Outer { get; }

            public T Value { get; }

            public ScopeItem(T value, ScopeItem? outer = null)
            {
                Id = Guid.NewGuid().ToString();

                Value = value;
                Outer = outer;
            }
        }
    }
}