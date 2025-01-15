using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal sealed class OwnedTypeTest : DbContextTest
    {
        private static async Task CreateSeedAsync(TestDbContext dbContext)
        {
            AddIfNotExists(dbContext, new Order(2, "b", "s", "c"));

            await dbContext.SaveChangesAsync();
        }

        public static async Task Search_QueryByOwnedType_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            await CreateSeedAsync(dbContext);

            var city = "c";
            var order = await dbContext.Orders.Where(o => o.StreetAddress.City == city).FirstOrDefaultAsync();
        }
    }
}
