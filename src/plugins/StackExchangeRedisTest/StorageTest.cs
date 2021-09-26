using System.Text;
using System.Threading.Tasks;

namespace StackExchangeRedisTest
{
    internal class StorageTest
    {
        public static async Task StorageString_Test()
        {
            var database = DatabaseProvider.GetDatabase();

            var data = "StorageString_Test";
            database.StringSet("test1", data);

            var value = await database.StringGetAsync("test1");
            var type = value.GetStorageType();

            //type = 4
            //字符串转为 byte[] 存储
        }

        public static async Task StorageBytes_Test()
        {
            var database = DatabaseProvider.GetDatabase();

            var data = "StorageBytes_Test";
            var array = Encoding.UTF8.GetBytes(data);
            database.StringSet("test2", array);

            var value = await database.StringGetAsync("test2");
            var type = value.GetStorageType();

            //type = 4
        }
    }
}
