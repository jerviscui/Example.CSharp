using System.Threading.Tasks;

namespace EfCoreTest
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            //EntityMappingTest.OnDelete_MsSql_Test();
            //OnDeleteTest.PostgreSql_Test();

            await SearchTest.ProtectedProp_Test();
        }
    }
}
