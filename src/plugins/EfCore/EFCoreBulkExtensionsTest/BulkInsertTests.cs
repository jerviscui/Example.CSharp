using EFCore.BulkExtensions;
using EfCoreTest;

namespace EFCoreBulkExtensionsTest;

internal class BulkInsertTests : DbContextTest
{
    public static async Task BulkInsert_Test()
    {
        var context = CreateMsSqlDbContext();

        var list = new List<Person>();
        for (int i = 0; i < 5000; i++)
        {
            list.Add(new Person(Consts.BaseId + i, $"name_{i}", 1, teacherId: 2));
        }

        await context.BulkInsertAsync(list);
        //Microsoft.Data.SqlClient.SqlException 违反了 PRIMARY KEY 约束“PK_Persons”

        //insert bulk [dbo].[Persons] ([Id] BigInt, [Name] NVarChar(max) COLLATE Chinese_PRC_CI_AS, [TeacherId] BigInt, [FamilyId] BigInt)
    }

    public static async Task BulkInsert_PostgreSql_Test()
    {
        var context = CreatePostgreSqlDbContext();

        var list = new List<Person>();
        for (int i = 0; i < 5000; i++)
        {
            list.Add(new Person(Consts.BaseId + i, $"name_{i}", 1, teacherId: 2));
        }

        await context.BulkInsertAsync(list);
        //Npgsql.PostgresException:“23505: 重复键违反唯一约束"PK_Persons"

        //COPY "Persons" ("Id", "FamilyId", "Name", "TeacherId") FROM STDIN (FORMAT BINARY);
    }

    public static async Task BulkInsertOrUpdate_Test()
    {
        var context = CreateMsSqlDbContext();

        var list = new List<Person>();
        for (int i = 0; i < 5000; i++)
        {
            list.Add(new Person(Consts.BaseId + i, $"name_{i}_{DateTime.Now.ToShortTimeString()}", 1, teacherId: 2));
        }

        await context.BulkInsertOrUpdateAsync(list);

        //使用临时表 和 MERGE
        //SELECT TOP 0 T.[Id], T.[Decimal], T.[FamilyId], T.[Long], T.[Name], T.[TeacherId] INTO[dbo].[PersonsTempd1e1ab90] FROM[dbo].[Persons] AS T LEFT JOIN[dbo].[Persons] AS Source ON 1 = 0;
        //insert bulk [dbo].[PersonsTempd1e1ab90] ([Id] BigInt, [Decimal] Decimal(18,2), [FamilyId] BigInt, [Long] BigInt, [Name] NVarChar(max) COLLATE Chinese_PRC_CI_AS, [TeacherId] BigInt)
        //MERGE [dbo].[Persons] WITH (HOLDLOCK) AS T USING (SELECT TOP 5000 * FROM [dbo].[PersonsTempd1e1ab90] ORDER BY [Id]) AS S ON T.[Id] = S.[Id] WHEN NOT MATCHED BY TARGET THEN INSERT ([Id], [Decimal], [FamilyId], [Long], [Name], [TeacherId]) VALUES (S.[Id], S.[Decimal], S.[FamilyId], S.[Long], S.[Name], S.[TeacherId]) WHEN MATCHED AND EXISTS (SELECT S.[Id], S.[Decimal], S.[FamilyId], S.[Long], S.[Name], S.[TeacherId] EXCEPT SELECT T.[Id], T.[Decimal], T.[FamilyId], T.[Long], T.[Name], T.[TeacherId]) THEN UPDATE SET T.[Decimal] = S.[Decimal], T.[FamilyId] = S.[FamilyId], T.[Long] = S.[Long], T.[Name] = S.[Name], T.[TeacherId] = S.[TeacherId];
        //IF OBJECT_ID ('[dbo].[PersonsTempd1e1ab90]', 'U') IS NOT NULL DROP TABLE [dbo].[PersonsTempd1e1ab90]
    }

    public static async Task BulkInsertOrUpdateOrDelete_Test()
    {
        var context = CreateMsSqlDbContext();

        var list = new List<Person>();
        for (int i = 0; i < 5000; i++)
        {
            list.Add(new Person(Consts.BaseId + i, $"name_{i}_{DateTime.Now.ToShortTimeString()}", 1, teacherId: 2));
        }

        await context.BulkInsertOrUpdateOrDeleteAsync(list);

        // MERGE 时使用 DELETE
        //SELECT TOP 0 T.[Id], T.[Decimal], T.[FamilyId], T.[Long], T.[Name], T.[TeacherId] INTO [dbo].[PersonsTemp8a057fef] FROM [dbo].[Persons] AS T LEFT JOIN [dbo].[Persons] AS Source ON 1 = 0;
        //insert bulk [dbo].[PersonsTemp8a057fef] ([Id] BigInt, [Decimal] Decimal(18,2), [FamilyId] BigInt, [Long] BigInt, [Name] NVarChar(max) COLLATE Chinese_PRC_CI_AS, [TeacherId] BigInt)
        //MERGE [dbo].[Persons] WITH (HOLDLOCK) AS T USING (SELECT TOP 5000 * FROM [dbo].[PersonsTemp8a057fef] ORDER BY [Id]) AS S ON T.[Id] = S.[Id] WHEN NOT MATCHED BY TARGET THEN INSERT ([Id], [Decimal], [FamilyId], [Long], [Name], [TeacherId]) VALUES (S.[Id], S.[Decimal], S.[FamilyId], S.[Long], S.[Name], S.[TeacherId]) WHEN MATCHED AND EXISTS (SELECT S.[Id], S.[Decimal], S.[FamilyId], S.[Long], S.[Name], S.[TeacherId] EXCEPT SELECT T.[Id], T.[Decimal], T.[FamilyId], T.[Long], T.[Name], T.[TeacherId]) THEN UPDATE SET T.[Decimal] = S.[Decimal], T.[FamilyId] = S.[FamilyId], T.[Long] = S.[Long], T.[Name] = S.[Name], T.[TeacherId] = S.[TeacherId] WHEN NOT MATCHED BY SOURCE THEN DELETE;
        //IF OBJECT_ID ('[dbo].[PersonsTemp8a057fef]', 'U') IS NOT NULL DROP TABLE [dbo].[PersonsTemp8a057fef]
    }
}
