using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Text;

namespace EfCoreTest;

internal sealed class SearchTest : DbContextTest
{

    #region Constants & Statics

    private static readonly Func<TestDbContext, long, Person> PersonById =
        EF.CompileQuery((TestDbContext context, long id) => context.Persons.First(o => o.Id == id));

    private static readonly Func<TestDbContext, long, Task<Person>> PersonByIdAsync =
        EF.CompileAsyncQuery((TestDbContext context, long id) => context.Persons.First(o => o.Id == id));

    private static readonly Func<TestDbContext, long, PersonDto> PersonByIdToDto =
        EF.CompileQuery(
        (TestDbContext context, long id) => context.Persons.Where(o => o.Id == id).Select(o => o.To()).First());

    private static readonly Func<TestDbContext, string, IEnumerable<Person>> PersonsByName =
        EF.CompileQuery((TestDbContext context, string name) => context.Persons.Where(o => o.Name.Contains(name)));

    private static readonly Func<TestDbContext, string, IAsyncEnumerable<Person>> PersonsByNameAsync =
        EF.CompileAsyncQuery((TestDbContext context, string name) => context.Persons.Where(o => o.Name.Contains(name)));

    private static async Task CreateSeedAsync(TestDbContext dbContext)
    {
        var family = new Family(2);
        AddIfNotExists(dbContext, family);

        var teacher = new Teacher(2, "teacher");
        AddIfNotExists(dbContext, teacher);

        for (var i = 0; i < 10000; i++)
        {
            var person = new Person(i + 100, $"name{i}", family.Id, teacher.Id);
            AddIfNotExists(dbContext, person);
        }

        await dbContext.SaveChangesAsync();
    }

    public static async Task CompileQuery_Include_Test()
    {
        await using var dbContext = CreatePostgreSqlDbContext();

        var query = EF.CompileQuery((TestDbContext context, string name) => context.Blogs.Include(o => o.BlogTags));

        var blogs = query(dbContext, "name").ToList();
    }

    public static async Task CompileQuery_Test()
    {
        await using var dbContext = CreateSqliteMemoryDbContext();

        var person1 = PersonById(dbContext, 1);
        var person2 = await PersonByIdAsync(dbContext, 1);

        var persons = PersonsByName(dbContext, "name");
        await foreach (var person in PersonsByNameAsync(dbContext, "name"))
        {
            Console.WriteLine(person.Name);
        }
    }

    public static async Task CompileQuery_WithSelect_Test()
    {
        await using var dbContext = CreateSqliteMemoryDbContext();

        var person = PersonByIdToDto(dbContext, 1);
    }

    public static async Task Exists_Test()
    {
        await using var dbContext = CreatePostgreSqlDbContext();

        var blogs = await dbContext.Blogs.Where(o => o.BlogTags.Any((x) => x.IsDelete)).ToListAsync();

        //Executed DbCommand(6ms) [Parameters= [], CommandType = 'Text', CommandTimeout = '30']
        //SELECT b."Id", b."Content", b."IsDelete", b."Title"
        //FROM "Blogs" AS b
        //WHERE EXISTS(
        //    SELECT 1
        //    FROM "BlogTags" AS b0
        //    WHERE b."Id" = b0."BlogId" AND b0."IsDelete")
    }

    public static async Task ProtectedProp_Test()
    {
        await using var dbContext = CreatePostgreSqlDbContext();

        var family = await dbContext.Families.FirstAsync();
        Console.WriteLine($"{family.Id} {family.Address}");

        var person = await dbContext.Persons.FirstAsync(o => o.Id == 2, default);
        Console.WriteLine($"{person.Name} {person.Long}");
        // EF 可以为 private 和 protected 属性赋值
    }

    public static async Task StreamingQuery_Test()
    {
        await using var dbContext = CreateSqliteMemoryDbContext();
        await CreateSeedAsync(dbContext);

        var stringBuilder = new StringBuilder();
        foreach (var person in dbContext.Persons.Where(o => o.Id > 100).Take(1000))
        {
            stringBuilder.Append(person.Name);
        }
        //todo: 检查内存使用情况
    }

    #endregion

}

public class PersonDto : Entity
{
    /// <inheritdoc/>
    public PersonDto(long id, string name)
    {
        Id = id;
        Name = name;
    }

    #region Properties

    public string Name { get; set; }

    #endregion

}

public static class Mapper
{

    #region Constants & Statics

    public static PersonDto To(this Person person)
    {
        return new(person.Id, person.Name);
    }

    #endregion

}
