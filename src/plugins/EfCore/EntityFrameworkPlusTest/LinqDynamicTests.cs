using EfCoreTest;
using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkPlusTest;

internal sealed class LinqDynamicTests : DbContextTest
{
    public static void WhereDynamic_ConstantArg_Test()
    {
        using var dbContext = CreateMsSqlDbContext();

        var list = dbContext.Persons.WhereDynamic(o => "o.Id < 3").ToListDynamic();

        //效果不如 Microsoft.EntityFrameworkCore.DynamicLinq
        //SELECT [p].[Id], [p].[Decimal], [p].[FamilyId], [p].[Long], [p].[Name], [p].[TeacherId]
        //FROM [Persons] AS [p]
        //WHERE [p].[Id] < CAST(3 AS bigint)
    }

    public static void WhereDynamic_ParameterizedArg_Test()
    {
        using var dbContext = CreateMsSqlDbContext();

        var list = dbContext.Persons.WhereDynamic(o => "o.Id < y", new { y = 3 }).ToListDynamic();

        //Executed DbCommand (63ms) [Parameters=[@__p_0='3'], CommandType='Text', CommandTimeout='30']
        //SELECT [p].[Id], [p].[Decimal], [p].[FamilyId], [p].[Long], [p].[Name], [p].[TeacherId]
        //FROM [Persons] AS [p]
        //WHERE [p].[Id] < @__p_0
    }

    public static async Task WhereDynamic_ToListAsync_Test()
    {
        using var dbContext = CreateMsSqlDbContext();

        var list = await dbContext.Persons.WhereDynamic(o => "o.Id < y", new { y = 3 }).SelectDynamic(o => "o.Name")
            .ToListAsync();

        //Executed DbCommand (81ms) [Parameters=[@__p_0='3'], CommandType='Text', CommandTimeout='30']
        //SELECT [p].[Name]
        //FROM [Persons] AS [p]
        //WHERE [p].[Id] < @__p_0
    }
}
