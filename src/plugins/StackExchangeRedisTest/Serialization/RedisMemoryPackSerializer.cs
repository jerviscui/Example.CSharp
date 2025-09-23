using MemoryPack;
using StackExchange.Redis.Extensions.Core;

namespace StackExchangeRedisTest;

public class RedisMemoryPackSerializer : ISerializer
{

    #region ISerializer implementations

    /// <inheritdoc/>
    public T? Deserialize<T>(byte[]? serializedObject)
    {
        if (serializedObject == null)
        {
            return default;
        }

        return MemoryPackSerializer.Deserialize<T>(serializedObject);
    }

    /// <inheritdoc/>
    public byte[] Serialize<T>(T? item)
    {
        if (EqualityComparer<T>.Default.Equals(item, default))
        {
            return [];
        }

        return MemoryPackSerializer.Serialize(item);
    }

    #endregion

}
