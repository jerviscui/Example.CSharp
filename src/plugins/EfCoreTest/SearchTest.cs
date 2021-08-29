using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal class SearchTest : DbContextTest
    {
        private static readonly Func<TestDbContext, long, Person> PersonById = EF.CompileQuery(
            (TestDbContext context, long id) =>
                context.Persons.First(o => o.Id == id));

        private static readonly Func<TestDbContext, long, Task<Person>> PersonByIdAsync = EF.CompileAsyncQuery(
            (TestDbContext context, long id) =>
                context.Persons.First(o => o.Id == id));

        public static async Task ProtectedProp_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var family = await dbContext.Families.FirstAsync();
            var person = await dbContext.Persons.FirstAsync();
        }

        public static async Task CompileQuery_PersonById_Test()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var person1 = PersonById(dbContext, 1);
            var person2 = await PersonByIdAsync(dbContext, 1);
        }

        public static void CompileQuery_PersonById2_Test()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            Func<TestDbContext, long, Person> query1 =
                EF.CompileQuery((TestDbContext context, long id) => context.Persons.Find(id));
            Func<TestDbContext, long, IEnumerable<Person>> query4 = EF.CompileQuery((TestDbContext context, long id) =>
                context.Persons.Where(o => o.Id == id).ToList());

            Func<TestDbContext, long, Person> query2 = EF.CompileQuery((TestDbContext context, long id) =>
                context.Persons.First(o => o.Id == id));
            Func<TestDbContext, long, IEnumerable<Person>> query3 = EF.CompileQuery((TestDbContext context, long id) =>
                context.Persons.Where(o => o.Id == id));

            //query1(dbContext, 1);
            query2(dbContext, 1);
            query3(dbContext, 1);
            //query4(dbContext, 1);

            Func<TestDbContext, long, Task<Person>> query11 = EF.CompileAsyncQuery((TestDbContext context, long id) =>
                context.Persons.First(o => o.Id == id));
            Func<TestDbContext, long, IAsyncEnumerable<Person>> query12 =
                EF.CompileAsyncQuery((TestDbContext context, long id) => context.Persons.Where(o => o.Id == id));

            query11(dbContext, 1);
            query12(dbContext, 1);
        }
    }
}
