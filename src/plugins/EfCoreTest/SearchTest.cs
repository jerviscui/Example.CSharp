using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal class SearchTest : DbContextTest
    {
        //private static readonly Func<TestDbContext, long, Person> PersonById =
        //    EF.CompileQuery((TestDbContext context, long id) =>
        //        context.Persons.Find(id));
        //Method 'EfCoreTest.Person Find(System.Object[])' declared on type
        //'Microsoft.EntityFrameworkCore.DbSet`1[EfCoreTest.Person]' cannot be called
        //with instance of type 'System.Linq.IQueryable`1[EfCoreTest.Person]'

        private static readonly Func<TestDbContext, long, IEnumerable<Person>> PersonById =
            EF.CompileQuery((TestDbContext context, long id) =>
                context.Persons.Where(o => o.Id == id));

        public static async Task ProtectedProp_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var family = await dbContext.Families.FirstAsync();
            var person = await dbContext.Persons.FirstAsync();
        }

        //private static Func<TestDbContext, long, Task<Person>> _personByIdAsync =
        //    EF.CompileAsyncQuery(async (TestDbContext context, long id) => await context.Persons.FindAsync(id));

        public static void CompileQuery_PersonById_Test()
        {
            using var dbContext = CreateSqliteMemoryDbContext();

            var person = PersonById(dbContext, 1).FirstOrDefault();
        }
    }
}
