namespace EfCoreTest;

internal sealed class RowVersionTest : DbContextTest
{

    #region Constants & Statics

    public static async Task MssqlRowVersion_Update_Test()
    {
        await using var dbContext = CreateMsSqlDbContext();

        var entity = new MssqlRowVersion { Name = "1" };
        await dbContext.MssqlRowVersions.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        //插入时自动生成 RowVersion
        //Executed DbCommand (33ms) [Parameters=[@p0='1' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
        //SET NOCOUNT ON;
        //INSERT INTO [MssqlRowVersions] ([Name])
        //VALUES (@p0);
        //SELECT [Id], [RowVersion]
        //FROM [MssqlRowVersions]
        //WHERE @@ROWCOUNT = 1 AND [Id] = scope_identity();

        entity.Name = "2";
        await dbContext.SaveChangesAsync();

        //更新时自动检查 RowVersion
        //Executed DbCommand (4ms) [Parameters=[@p1='1', @p0='2' (Nullable = false) (Size = 4000), @p2='0x00000000000007D1' (Nullable = false) (Size = 8)], CommandType='Text', CommandTimeout='30']
        //SET NOCOUNT ON;
        //UPDATE [MssqlRowVersions] SET [Name] = @p0
        //WHERE [Id] = @p1 AND [RowVersion] = @p2;
        //SELECT [RowVersion]
        //FROM [MssqlRowVersions]
        //WHERE @@ROWCOUNT = 1 AND [Id] = @p1;
    }

    public static async Task PgsqlRowVersion_Update_Test()
    {
        await using var dbContext = CreatePostgreSqlDbContext();

        var entity = new PgsqlRowVersion { Name = "1" };
        await dbContext.PgsqlRowVersions.AddAsync(entity);
        await dbContext.SaveChangesAsync();

        //插入时自动生成 xmin
        //Executed DbCommand(56ms) [Parameters= [@p0 = '1'(Nullable = false)], CommandType = 'Text', CommandTimeout = '30']
        //INSERT INTO "PgsqlRowVersions"("Name")
        //VALUES(@p0)
        //RETURNING "Id", xmin;

        entity.Name = "2";
        await dbContext.SaveChangesAsync();

        //更新时自动检查 xmin
        //Executed DbCommand(8ms) [Parameters= [@p1 = '1', @p0 = '2'(Nullable = false), @p2 = '6840'(DbType = Object)], CommandType = 'Text', CommandTimeout = '30']
        //UPDATE "PgsqlRowVersions" SET "Name" = @p0
        //WHERE "Id" = @p1 AND xmin = @p2
        //RETURNING xmin;

        // 7.0后不再创建字段
        //var xmin = dbContext.Entry(entity).Property("xmin");
    }

    #endregion

}
