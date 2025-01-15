using EFCore.BulkExtensions;
using EfCoreTest;

namespace EFCoreBulkExtensionsTest;

internal sealed class BulkDeleteTests : DbContextTest
{
    public static async Task BatchDelete_Test()
    {
        var context = CreateMsSqlDbContext();

        await context.Persons.Where(o => o.Id >= Consts.BaseId).BatchDeleteAsync();

        //Executed DbCommand(210ms) [Parameters=[], CommandType = 'Text', CommandTimeout = '30']
        //DELETE[p]
        //FROM[Persons] AS[p]
        //WHERE[p].[Id] >= CAST(10000 AS bigint)
    }

    public static async Task BatchDelete_PostgreSql_Test()
    {
        var context = CreatePostgreSqlDbContext();

        await context.Persons.Where(o => o.Id >= Consts.BaseId).BatchDeleteAsync();
    }
}
