using EFCore.BulkExtensions;
using EfCoreTest;

namespace EFCoreBulkExtensionsTest;

internal class BulkUpdateTests : DbContextTest
{
    public static async Task BulkUpdate_Test()
    {
        var context = CreateMsSqlDbContext();

        var list = new List<Person>();
        for (int i = 0; i < 10; i++)
        {
            list.Add(new Person(Consts.BaseId + i, $"1name_u_{i}", 1, teacherId: 1));
        }

        await context.BulkUpdateAsync(list,
            config =>
            {
                config.PropertiesToInclude = new List<string> { nameof(Person.Name) };
            });

        //insert bulk [dbo].[PersonsTempb48685f6] ([Id] BigInt, [Name] NVarChar(max) COLLATE Chinese_PRC_CI_AS)
        //MERGE [dbo].[Persons] WITH (HOLDLOCK) AS T USING (SELECT TOP 10 * FROM [dbo].[PersonsTempb48685f6] ORDER BY [Id]) AS S ON T.[Id] = S.[Id] WHEN MATCHED AND EXISTS (SELECT S.[Id], S.[Name] EXCEPT SELECT T.[Id], T.[Name]) THEN UPDATE SET T.[Name] = S.[Name];
    }

    public static async Task BatchUpdate_updateExpression_Test()
    {
        var context = CreateMsSqlDbContext();

        await context.Persons.Where(o => o.Id >= Consts.BaseId && o.Id < Consts.BaseId + 500)
            .BatchUpdateAsync(p => new Person(p.Id, "", p.FamilyId, p.TeacherId) { Name = p.Name + "_u" });

        //Executed DbCommand(135ms) [Parameters=[@param_0 = '_u'(Nullable = false)(Size = 4000)], CommandType = 'Text', CommandTimeout = '30']
        //UPDATE p SET[p].[Name] = [p].[Name] + @param_0
        //FROM[Persons] AS[p]
        //WHERE([p].[Id] > CAST(10000 AS bigint)) AND([p].[Id] < CAST(10500 AS bigint))
    }

    public static async Task BatchUpdate_updateExpression_DynamicValue_Test()
    {
        var context = CreateMsSqlDbContext();

        await context.Persons.Where(o => o.Id >= Consts.BaseId && o.Id < Consts.BaseId + 500)
            .BatchUpdateAsync(p =>
                new Person(p.Id, "", p.FamilyId, p.TeacherId) { Name = p.Name + DateTime.Now.ToShortTimeString() });

        //Executed DbCommand(107ms) [Parameters=[@param_0 = '18:44'(Nullable = false)(Size = 4000)], CommandType = 'Text', CommandTimeout = '30']
        //UPDATE p SET[p].[Name] = [p].[Name] + @param_0
        //FROM[Persons] AS[p]
        //WHERE([p].[Id] >= CAST(10000 AS bigint)) AND([p].[Id] < CAST(10500 AS bigint))
    }
}
