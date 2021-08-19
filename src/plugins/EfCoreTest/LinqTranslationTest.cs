using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal class LinqTranslationTest : DbContextTest
    {
        private static async Task CreateSeedAsync(TestDbContext dbContext)
        {
            var family = new Family(2);
            AddIfNotExists(dbContext, family);

            var teacher = new Teacher(2, "teacher");
            AddIfNotExists(dbContext, teacher);

            for (int i = 0; i < 10000; i++)
            {
                var person = new Person(i + 100, $"name{i}", family.Id, teacher.Id);
                AddIfNotExists(dbContext, person);
            }

            await dbContext.SaveChangesAsync();
        }

        public static async Task ListContains_Predicate_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            await CreateSeedAsync(dbContext);

            var persons = await dbContext.Persons.AsNoTracking().Take(5000).ToListAsync();

            var list = dbContext.Persons.Where(o => persons.Select(p => p.Name).Contains(o.Name)).ToList();
        }

        public static async Task ListAny_Predicate_ThrowInvalidOperationException_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            await CreateSeedAsync(dbContext);

            var persons = await dbContext.Persons.AsNoTracking().Take(50).ToListAsync();

            try
            {
                var list = dbContext.Persons.Where(o => persons.Any(p => p.Name == o.Name)).ToList();
            }
            catch (InvalidOperationException e)
            {
                Console.WriteLine(e);
            }
        }

        public static async Task ListAny_Predicate_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            await CreateSeedAsync(dbContext);

            var persons = await dbContext.Persons.AsNoTracking().Take(50).Select(o => o.Name).ToListAsync();

            var list = dbContext.Persons.Where(o => persons.Any(p => p == o.Name)).ToList();
        }
    }
}
