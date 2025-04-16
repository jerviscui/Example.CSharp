using Microsoft.EntityFrameworkCore;

namespace EfCoreTest;

internal sealed class TableSplittingTests : DbContextTest
{

    #region Constants & Statics
    private static async Task CreateSeedAsync(TestDbContext dbContext)
    {
        var order = new SplitOrder(1)
        {
            Status = OrderStatus.Pending,
            DetailedSplitOrder = new DetailedSplitOrder
            {
                Status = OrderStatus.Pending,
                ShippingAddress = "221 B Baker St, London",
                BillingAddress = "11 Wall Street, New York"
            }
        };

        AddIfNotExists(dbContext, order);

        await dbContext.SaveChangesAsync();
    }

    public static async Task Insert_Test()
    {
        using var dbContext = CreateMsSqlDbContext();

        await CreateSeedAsync(dbContext);

        //Executed DbCommand (57ms) [Parameters=[@p0='1', @p1='0' (Nullable = true), @p2='11 Wall Street, New York' (Nullable = false) (Size = 4000), @p3='221 B Baker St, London' (Nullable = false) (Size = 4000)], CommandType='Text', CommandTimeout='30']
        //SET NOCOUNT ON;
        //INSERT INTO [SplitOrder] ([Id], [Status], [BillingAddress], [ShippingAddress])
        //VALUES (@p0, @p1, @p2, @p3);
    }

    public static async Task Search_DetailedSplitOrder_Test()
    {
        using var dbContext = CreateMsSqlDbContext();
        await CreateSeedAsync(dbContext);

        var detail = await dbContext.DetailedSplitOrders.FirstOrDefaultAsync(o => o.Id == 1);

        //SELECT TOP(1) [s].[Id], [s].[BillingAddress], [s].[ShippingAddress], [s].[Status]
        //FROM [SplitOrder] AS [s]
        //INNER JOIN [SplitOrder] AS [s0] ON [s].[Id] = [s0].[Id]
        //WHERE [s].[Id] = CAST(1 AS bigint)
    }

    public static async Task Search_SplitOrder_Test()
    {
        using var dbContext = CreateMsSqlDbContext();
        await CreateSeedAsync(dbContext);

        var count = await dbContext.SplitOrders.CountAsync(o => o.Status == OrderStatus.Pending);

        //SELECT COUNT(*)
        //FROM [SplitOrder] AS [s]
        //WHERE [s].[Status] = 0
    }

    public static void TableSplitting_MsSql_Test()
    {
        using var dbContext = CreateMsSqlDbContext();

        //CREATE TABLE [SplitOrder] (
        //    [Id] bigint NOT NULL IDENTITY,
        //    [Status] int NULL,
        //    [BillingAddress] nvarchar(max) NOT NULL,
        //    [ShippingAddress] nvarchar(max) NOT NULL,
        //    CONSTRAINT [PK_SplitOrder] PRIMARY KEY ([Id])
        //);
    }

    public static void TableSplitting_PostgreSql_Test()
    {
        using var dbContext = CreatePostgreSqlDbContext();

    }
    public static void TableSplitting_Sqlite_Test()
    {
        using var dbContext = CreateSqliteMemoryDbContext();

        //CREATE TABLE "SplitOrder" (
        //  "Id" INTEGER NOT NULL CONSTRAINT "PK_SplitOrder" PRIMARY KEY,
        //  "Status" INTEGER NULL,
        //  "BillingAddress" TEXT NOT NULL,
        //  "ShippingAddress" TEXT NOT NULL
        //);
    }
    #endregion

}
