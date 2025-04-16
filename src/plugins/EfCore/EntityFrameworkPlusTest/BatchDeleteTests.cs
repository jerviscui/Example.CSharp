using EfCoreTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Z.EntityFramework.Plus;

namespace EntityFrameworkPlusTest;

internal sealed class BatchDeleteTests : DbContextTest
{
    public static async Task DeleteAsync_MsSql_Test()
    {
        using var dbContext = CreateMsSqlDbContext();

        var deleted = await dbContext.Persons.Where(o => o.Id < 3).DeleteAsync(delete =>
        {
            delete.Executing = command =>
            {
                var diagnosticsLogger =
                    dbContext.Database.GetService<IRelationalCommandDiagnosticsLogger>();
                diagnosticsLogger.Logger.LogInformation(command.CommandText);
            };
        });

        //DECLARE @stop int
        //DECLARE @rowAffected INT
        //DECLARE @totalRowAffected INT
        //
        //SET @stop = 0
        //SET @totalRowAffected = 0
        //
        //WHILE @stop=0
        //    BEGIN
        //        DELETE TOP (4000)
        //        FROM    A
        //        FROM    [Persons] AS A
        //                INNER JOIN ( SELECT [p].[Id]
        //FROM [Persons] AS [p]
        //WHERE [p].[Id] < CAST(3 AS bigint)
        //                           ) AS B ON A.[Id] = B.[Id]
        //
        //        SET @rowAffected = @@ROWCOUNT
        //        SET @totalRowAffected = @totalRowAffected + @rowAffected
        //
        //        IF @rowAffected < 4000
        //            SET @stop = 1
        //    END
        //
        //SELECT  @totalRowAffected
    }

    public static async Task DeleteAsync_WithParameters_MsSql_Test()
    {
        using var dbContext = CreateMsSqlDbContext();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var deleted = await dbContext.Persons.Where(o => o.Id > 100).DeleteAsync(delete =>
        {
            delete.BatchSize = 50;
            delete.BatchDelayInterval = 5_000;
            delete.Executing = command =>
            {
                var diagnosticsLogger =
                    dbContext.Database.GetService<IRelationalCommandDiagnosticsLogger>();
                diagnosticsLogger.Logger.LogInformation(command.CommandText);
            };
        });

        //DECLARE @stop int
        //DECLARE @rowAffected INT
        //DECLARE @totalRowAffected INT
        //
        //SET @stop = 0
        //SET @totalRowAffected = 0
        //
        //WHILE @stop=0
        //    BEGIN
        //        IF @rowAffected IS NOT NULL
        //            BEGIN
        //                WAITFOR DELAY '00:00:05:000'
        //            END
        //
        //        DELETE TOP (50)
        //        FROM    A
        //        FROM    [Persons] AS A
        //                INNER JOIN ( SELECT [p].[Id]
        //FROM [Persons] AS [p]
        //WHERE [p].[Id] > CAST(100 AS bigint)
        //                           ) AS B ON A.[Id] = B.[Id]
        //
        //        SET @rowAffected = @@ROWCOUNT
        //        SET @totalRowAffected = @totalRowAffected + @rowAffected
        //
        //        IF @rowAffected < 50
        //            SET @stop = 1
        //    END
        //
        //SELECT  @totalRowAffected
    }

    public static async Task DeleteAsync_ChangeTracking_MsSql_DbUpdateConcurrencyException_Test()
    {
        using var dbContext = CreateMsSqlDbContext();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var first = await dbContext.Persons.Where(o => o.Id == 1).FirstAsync();

        var deleted = await dbContext.Persons.Where(o => o.Id < 3).DeleteAsync(delete =>
        {
            delete.BatchSize = 1000;
            delete.BatchDelayInterval = 500;
            delete.Executing = command =>
            {
                var diagnosticsLogger =
                    dbContext.Database.GetService<IRelationalCommandDiagnosticsLogger>();
                diagnosticsLogger.Logger.LogInformation(command.CommandText);
            };
        });

        first.SetProp(1, 1);
        dbContext.Persons.Update(first);
        await dbContext.SaveChangesAsync(); //throw Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException
    }

    public static async Task DeleteAsync_WithParameters_PostgreSql_Test()
    {
        using var dbContext = CreatePostgreSqlDbContext();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();

        var deleted = await dbContext.Persons.Where(o => o.Id > 100).DeleteAsync(delete =>
        {
            delete.BatchSize = 50;
            delete.BatchDelayInterval = 5_000;
            delete.Executing = command =>
            {
                var diagnosticsLogger =
                    dbContext.Database.GetService<IRelationalCommandDiagnosticsLogger>();
                diagnosticsLogger.Logger.LogInformation(command.CommandText);
            };
        });

        //DELETE FROM [Persons] AS A
        //USING ( SELECT p."Id"
        //FROM "Persons" AS p
        //WHERE p."Id" > 100 ) AS B WHERE A."Id" = B."Id"
    }
}
