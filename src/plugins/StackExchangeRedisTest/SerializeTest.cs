using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace StackExchangeRedisTest
{
    internal class SerializeTest
    {
        public static async Task SystemTextJson_Test()
        {
            var database = DatabaseProvider.GetDatabase();

            database.StringSet("Serialize1", JsonSerializer.SerializeToUtf8Bytes(new MyClass { Name = "ser1" }));

            var value = await database.StringGetAsync("Serialize1");
            var type = value.GetStorageType(); //is byte array
            var data = JsonSerializer.Deserialize<MyClass>((byte[])value);
        }

        public static async Task NewtownJson_Test()
        {
            var database = DatabaseProvider.GetDatabase();

            database.StringSet("Serialize2", JsonConvert.SerializeObject(new MyClass { Name = "ser2" }));

            var value = await database.StringGetAsync("Serialize2");
            var type = value.GetStorageType();
            var data = JsonConvert.DeserializeObject<MyClass>(value); //value to string
        }

        private class MyClass
        {
            public string Name { get; set; } = "test name";

            [JsonInclude]
            [JsonProperty]
            public string Address { get; private set; } = "test address";
        }
    }
}
