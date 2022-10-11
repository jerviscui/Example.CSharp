using EFCore.BulkExtensions;
using EfCoreTest;

namespace EFCoreBulkExtensionsTest;

internal class BulkReadTests : DbContextTest
{
    public static async Task BulkRead_Test()
    {
        var context = CreateMsSqlDbContext();

        var list = new List<Person>();
        for (int i = 0; i < 2000; i++)
        {
            list.Add(new Person(Consts.BaseId + i, "", 0, 0));
        }

        await context.BulkReadAsync(list, config =>
        {
            config.UpdateByProperties = new List<string> { nameof(Person.Id) };
        });

        //SELECT TOP 0 T.[Id] INTO [dbo].[PersonsTemp1af1ffd6] FROM [dbo].[Persons] AS T LEFT JOIN [dbo].[Persons] AS Source ON 1 = 0;
        //insert bulk [dbo].[PersonsTemp1af1ffd6] ([Id] BigInt)
        //SELECT S.[Id], S.[Decimal], S.[FamilyId], S.[Long], S.[Name], S.[TeacherId] FROM [dbo].[Persons] AS S JOIN [dbo].[PersonsTemp1af1ffd6] AS J ON S.[Id] = J.[Id]
        //IF OBJECT_ID ('[dbo].[PersonsTemp1af1ffd6]', 'U') IS NOT NULL DROP TABLE [dbo].[PersonsTemp1af1ffd6]
    }
}
