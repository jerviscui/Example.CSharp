using System.Data.Common;
using System.Text;
using EfCoreTest;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace EntityFrameworkPlusTest;

internal class BatchUpdateTests : DbContextTest
{
    private static string GetCommandText(DbCommand command)
    {
        var sb = new StringBuilder();

        foreach (DbParameter parameter in command.Parameters)
        {
            sb.Append(parameter.ParameterName);
            sb.Append('=');
            if (parameter is SqlParameter p)
            {
                sb.Append(p.SqlValue);
            }
            sb.AppendLine();
        }

        sb.AppendLine(command.CommandText);

        return sb.ToString();
    }

    public static async Task UpdateAsync_MsSql_Test()
    {
        var dbContext = CreateMsSqlDbContext();

        var updated = await dbContext.Persons.Where(o => o.Id < 3).UpdateAsync(
            // ReSharper disable once UseStringInterpolation
            o => new { Name = o.Name + "_m_" + o.Id }, //不能使用插值表达式
            update =>
            {
                update.Executing = command =>
                {
                    var diagnosticsLogger =
                        dbContext.Database.GetService<IRelationalCommandDiagnosticsLogger>();
                    var commandText = GetCommandText(command);
                    diagnosticsLogger.Logger.LogInformation(commandText);
                };
            });

        //UPDATE A 
        //SET A.[Name] = (B.[Name] + N'_m_') + CAST(B.[Id] AS nvarchar(max))
        //FROM [Persons] AS A
        //INNER JOIN ( 
        //    SELECT [p].[Id], [p].[Decimal], [p].[FamilyId], [p].[Long], [p].[Name], [p].[TeacherId]
        //    FROM [Persons] AS [p]
        //    WHERE [p].[Id] < CAST(3 AS bigint)
        //           ) AS B ON A.[Id] = B.[Id]
    }

    public static async Task UpdateAsync_WithParameters_MsSql_Test()
    {
        var dbContext = CreateMsSqlDbContext();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var updated = await dbContext.Persons.Where(o => o.Id > 100).UpdateAsync(
            o => new { Name = o.Name + "_m_" + o.Id }, //不能使用插值表达式
            update =>
            {
                update.BatchSize = 50;
                update.UseTableLock = true;
                update.Executing = command =>
                {
                    var diagnosticsLogger =
                        dbContext.Database.GetService<IRelationalCommandDiagnosticsLogger>();
                    var commandText = GetCommandText(command);
                    diagnosticsLogger.Logger.LogInformation(commandText);
                };
            });

        //DECLARE @stop INT
        //DECLARE @rowAffected INT
        //DECLARE @totalRowAffected INT
        //DECLARE @ZZZ_INDEX INT
        //DECLARE @ZZZ_MAX INT
        //
        //SET @stop = 0
        //SET @totalRowAffected = 0
        //SET @ZZZ_INDEX = 0
        //
        //SELECT Z_Batch.[Id],
        //       ROW_NUMBER() OVER (ORDER BY (SELECT 1)) AS ZZZ_INDEX
        //INTO #ZZZBatch_10a367e0_d325_4f51_a871_a004b820edee
        //FROM ( SELECT [p].[Id], [p].[Decimal], [p].[FamilyId], [p].[Long], [p].[Name], [p].[TeacherId]
        //FROM [Persons] AS [p]
        //WHERE [p].[Id] > CAST(100 AS bigint) ) AS Z_Batch
        //
        //SELECT @ZZZ_MAX = MAX(ZZZ_INDEX)
        //FROM #ZZZBatch_10a367e0_d325_4f51_a871_a004b820edee
        //
        //WHILE @ZZZ_INDEX < @ZZZ_MAX
        //BEGIN
        //    UPDATE A WITH ( TABLOCK )
        //    SET A.[Name] = (B.[Name] + N'_m_') + CAST(B.[Id] AS nvarchar(max))
        //    FROM [Persons] AS A
        //    INNER JOIN ( SELECT * FROM #ZZZBatch_10a367e0_d325_4f51_a871_a004b820edee AS ZZZ_temp
        //                WHERE ZZZ_temp.ZZZ_INDEX > @ZZZ_INDEX AND ZZZ_temp.ZZZ_INDEX <= @ZZZ_INDEX + 50
        //               ) AS ZZZ_Batch ON A.[Id] = ZZZ_Batch.[Id]
        //    INNER JOIN ( SELECT [p].[Id], [p].[Decimal], [p].[FamilyId], [p].[Long], [p].[Name], [p].[TeacherId]
        //FROM [Persons] AS [p]
        //WHERE [p].[Id] > CAST(100 AS bigint)
        //               ) AS B ON A.[Id] = B.[Id]
        //
        //    SET @rowAffected = @@ROWCOUNT
        //    SET @ZZZ_INDEX = @ZZZ_INDEX + 50
        //    SET @totalRowAffected = @totalRowAffected + @rowAffected
        //END
        //
        //SELECT @totalRowAffected
    }

    public static async Task UpdateAsync_WithParameters_PostgreSql_Test()
    {
        var dbContext = CreatePostgreSqlDbContext();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var updated = await dbContext.Persons.Where(o => o.Id > 100).UpdateAsync(
            o => new { Name = o.Name + "_m_" + o.Id }, //不能使用插值表达式
            update =>
            {
                update.BatchSize = 50;
                update.UseTableLock = true;
                update.Executing = command =>
                {
                    var diagnosticsLogger =
                        dbContext.Database.GetService<IRelationalCommandDiagnosticsLogger>();
                    var commandText = GetCommandText(command);
                    diagnosticsLogger.Logger.LogInformation(commandText);
                };
            });

        //UPDATE "Persons"
        //SET "Name" = ("Name" || '_m_') || "Id"::text
        //WHERE EXISTS ( 
        //    SELECT 1 FROM (SELECT p."Id", p."Decimal", p."FamilyId", p."Long", p."Name", p."TeacherId"
        //        FROM "Persons" AS p
        //        WHERE p."Id" > 100) B
        //    WHERE "Persons"."Id" = B."Id"
        //            )
    }
}
