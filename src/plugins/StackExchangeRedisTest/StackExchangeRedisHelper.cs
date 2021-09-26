using System.Reflection;
using StackExchange.Redis;

namespace StackExchangeRedisTest
{
    public static class StackExchangeRedisHelper
    {
        /// <summary>
        /// Gets RedisValue StorageType.
        /// <para>
        /// internal enum StorageType
        /// {
        ///     Null, Int64, UInt64, Double, Raw, String,
        /// }
        /// </para>
        /// </summary>
        /// <param name="value">The value.</param>
        public static int GetStorageType(this RedisValue value)
        {
            var property = typeof(RedisValue).GetProperty("Type", BindingFlags.NonPublic | BindingFlags.Instance)!;
            var type = (int)property.GetValue(value)!;

            return type;
        }
    }
}
