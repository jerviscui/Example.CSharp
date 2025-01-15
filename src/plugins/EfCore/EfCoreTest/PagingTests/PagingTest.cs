using EfCoreTest.Paging;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest;

internal sealed class PagingTest : DbContextTest
{

    #region Constants & Statics
    public static async Task MsSql_NoOffset_Test()
    {
        var dbContext = CreateMsSqlDbContext();

        var count = await dbContext.FactSales.AsNoTracking().CountAsync();

        var first = await dbContext.FactSales.AsNoTracking().OrderBy(o => o.Id).Take(10).ToListAsync();

        var second = await dbContext.FactSales.AsNoTracking().Where(o => o.Id > first.Last().Id).OrderBy(o => o.Id)
            .Take(10).ToListAsync();

        //Executed DbCommand(73ms) [Parameters=[], CommandType = 'Text', CommandTimeout = '30']
        //SELECT COUNT(*)
        //FROM[fact_sales] AS[f]

        //Executed DbCommand(1ms) [Parameters=[@__p_0 = '10'], CommandType = 'Text', CommandTimeout = '30']
        //SELECT TOP(@__p_0) [f].[date_id], [f].[id], [f].[other_data], [f].[product_id], [f].[quantity], [f].[store_id], [f].[unit_price]
        //FROM[fact_sales] AS[f]
        //ORDER BY[f].[id]

        //Executed DbCommand(0ms) [Parameters=[@__p_1 = '10', @__Last_Id_0 = '10'], CommandType = 'Text', CommandTimeout = '30']
        //SELECT TOP(@__p_1) [f].[date_id], [f].[id], [f].[other_data], [f].[product_id], [f].[quantity], [f].[store_id], [f].[unit_price]
        //FROM[fact_sales] AS[f]
        //WHERE[f].[id] > @__Last_Id_0
        //ORDER BY[f].[id]
    }

    public static async Task MsSql_Test()
    {
        var dbContext = CreateMsSqlDbContext();

        var count = await dbContext.FactSales.AsNoTracking().CountAsync();

        var f = await dbContext.FactSales.AsNoTracking().Skip(count - 1).Take(1).ToListAsync();

        //200w 0.4s
        //exec sp_executesql N'SELECT [f].[date_id], [f].[other_data], [f].[product_id], [f].[quantity], [f].[store_id], [f].[unit_price]
        //FROM[fact_sales] AS[f]
        //ORDER BY(SELECT 1)
        //OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY',N'@__p_0 int, @__p_1 int',@__p_0=1999999,@__p_1=1
    }

    public static async Task PostgreSql_NoOffset_Test()
    {
        var dbContext = CreatePostgreSqlDbContext();

        var count = await dbContext.FactSales.AsNoTracking().CountAsync();

        var first = await dbContext.FactSales.AsNoTracking().OrderBy(o => o.Id).Take(10).ToListAsync();

        var second = await dbContext.FactSales.AsNoTracking().Where(o => o.Id > first.Last().Id).OrderBy(o => o.Id)
            .Take(10).ToListAsync();

        //Executed DbCommand(1,732ms) [Parameters=[], CommandType = 'Text', CommandTimeout = '30']
        //SELECT COUNT(*)::INT
        //FROM fact_sales AS f

        //Executed DbCommand(15ms)[Parameters =[@__p_0 = '10'], CommandType = 'Text', CommandTimeout = '30']
        //SELECT f.date_id, f.id, f.other_data, f.product_id, f.quantity, f.store_id, f.unit_price
        //FROM fact_sales AS f
        //ORDER BY f.id
        //LIMIT @__p_0

        //Executed DbCommand(15ms)[Parameters =[@__Last_Id_0 = '10', @__p_1 = '10'], CommandType = 'Text', CommandTimeout = '30']
        //SELECT f.date_id, f.id, f.other_data, f.product_id, f.quantity, f.store_id, f.unit_price
        //FROM fact_sales AS f
        //WHERE f.id > @__Last_Id_0
        //ORDER BY f.id
        //LIMIT @__p_1
    }

    public static async Task PostgreSql_Opetimization_Test()
    {
        var dbContext = CreatePostgreSqlDbContext();

        var count = await dbContext.FactSales.AsNoTracking().CountAsync();

        var ids = await dbContext.FactSales.AsNoTracking().OrderBy(o => o.DateId).ThenBy(o => o.Id).Take(10)
            .Select(o => o.Id).ToListAsync();

        var f = await dbContext.FactSales.AsNoTracking().Where(o => ids.Contains(o.Id)).ToListAsync();

        //Executed DbCommand(19ms) [Parameters=[@__p_0 = '10'], CommandType = 'Text', CommandTimeout = '30']
        //SELECT f.id
        //FROM fact_sales AS f
        //ORDER BY f.date_id, f.id
        //LIMIT @__p_0

        //Executed DbCommand(20ms)[Parameters =[@__ids_0 ={ '30', '60', '90', '120', '150', ... } (DbType = Object), @__p_1 = '10'], CommandType = 'Text', CommandTimeout = '30']
        //SELECT f.date_id, f.id, f.other_data, f.product_id, f.quantity, f.store_id, f.unit_price
        //FROM fact_sales AS f
        //WHERE f.id = ANY(@__ids_0)
        //LIMIT @__p_
    }

    public static async Task PostgreSql_Test()
    {
        var dbContext = CreatePostgreSqlDbContext();

        var count = await dbContext.FactSales.AsNoTracking().CountAsync();

        var f = await dbContext.FactSales.AsNoTracking().Skip(count - 1).Take(1).ToListAsync();

        //200w 2.7s
        //SELECT f.date_id, f.other_data, f.product_id, f.quantity, f.store_id, f.unit_price
        //FROM fact_sales AS f
        //LIMIT $1 OFFSET $2;
    }
    public static async Task SkipAll_Test(int pageIndex = 1, int pageSize = 10)
    {
        var dbContext = CreateMsSqlDbContext();

        var total = await dbContext.FactSales.AsNoTracking().CountAsync();

        var skip = (pageIndex - 1) * pageSize;

        var list = new List<FactSale>(0);
        if (skip < total)
        {
            list = await dbContext.FactSales.AsNoTracking().Skip(skip).Take(pageSize).ToListAsync();
        }
    }
    #endregion

}
