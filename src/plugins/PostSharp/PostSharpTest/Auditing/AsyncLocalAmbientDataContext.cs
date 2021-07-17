using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Volo.Abp.Threading
{
    //singleton
    [Serializable]
    public class AsyncLocalAmbientDataContext
    {
        private static readonly ConcurrentDictionary<string, AsyncLocal<object?>> AsyncLocalDictionary = new();

        private static readonly Func<string, AsyncLocal<object?>> ValueFac = (k) => new AsyncLocal<object?>();

        public void SetData(string key, object? value)
        {
            var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, ValueFac);
            asyncLocal.Value = value;
        }

        public object? GetData(string key)
        {
            var asyncLocal = AsyncLocalDictionary.GetOrAdd(key, ValueFac);
            return asyncLocal.Value;
        }
    }
}