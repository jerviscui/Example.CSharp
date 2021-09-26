using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace StackExchangeRedisTest
{
    internal class ScriptTest
    {
        private const string Script = @"
local key = @key
local value = @value

redis.call('SET', key, value)

return redis.call('GET', key)
";

        public static async Task ScriptEvaluate_ReturnBytes_Test()
        {
            var script = LuaScript.Prepare(Script);

            var key = "ScriptEvaluate_ReturnBytes";

            var database = DatabaseProvider.GetDatabase();
            var result = await database.ScriptEvaluateAsync(script,
                new { key = (RedisKey)key, value = Encoding.UTF8.GetBytes(key) });

            if (!result.IsNull && result.Type == ResultType.BulkString)
            {
                var data = Encoding.UTF8.GetString((byte[])result);
            }
        }
    }
}
