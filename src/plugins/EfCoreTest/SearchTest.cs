using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest
{
    internal class SearchTest : DbContextTest
    {
        private static readonly Func<TestDbContext, long, Person> PersonById =
            EF.CompileQuery((TestDbContext context, long id) => context.Persons.First(o => o.Id == id));

        private static readonly Func<TestDbContext, long, Task<Person>> PersonByIdAsync =
            EF.CompileAsyncQuery((TestDbContext context, long id) => context.Persons.First(o => o.Id == id));

        private static readonly Func<TestDbContext, string, IEnumerable<Person>> PersonsByName =
            EF.CompileQuery((TestDbContext context, string name) => context.Persons.Where(o => o.Name.Contains(name)));

        private static readonly Func<TestDbContext, string, IAsyncEnumerable<Person>> PersonsByNameAsync =
            EF.CompileAsyncQuery((TestDbContext context, string name) =>
                context.Persons.Where(o => o.Name.Contains(name)));

        public static async Task ProtectedProp_Test()
        {
            await using var dbContext = CreateMsSqlDbContext();

            var family = await dbContext.Families.FirstAsync();
            var person = await dbContext.Persons.FirstAsync();
        }

        public static async Task CompileQuery_Test()
        {
            await using var dbContext = CreateSqliteMemoryDbContext();

            var person1 = PersonById(dbContext, 1);
            var person2 = await PersonByIdAsync(dbContext, 1);

            var persons = PersonsByName(dbContext, "name");
            await foreach (var person in PersonsByNameAsync(dbContext, "name"))
            {
            }
        }

        private static readonly Func<TestDbContext, long, PersonDto> PersonByIdToDto =
            EF.CompileQuery((TestDbContext context, long id) =>
                context.Persons.Where(o => o.Id == id).Select(o => o.To()).First());

        public static async Task CompileQuery_WithSelect_Test()
        {
            await using var dbContext = CreateSqliteMemoryDbContext();

            var person = PersonByIdToDto(dbContext, 1);
        }

        public static async Task CompileQuery_Include_Test()
        {
            await using var dbContext = CreateSqliteMemoryDbContext();

            var query = EF.CompileQuery((TestDbContext context, string name) =>
                context.Blogs.Include(o => o.BlogTags));

            var blogs = query(dbContext, "name");
        }
    }

    public class PersonDto : Entity
    {
        /// <inheritdoc />
        public PersonDto(long id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; set; }
    }

    public static class Mapper
    {
        public static PersonDto To(this Person person)
        {
            return new(person.Id, person.Name);
        }
    }
}
