using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal class SearchTest : DbContextTest
    {
        public static async Task ProtectedProp_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var family = await dbContext.Families.FirstAsync();
            var person = await dbContext.Persons.FirstAsync();
        }
    }
}
