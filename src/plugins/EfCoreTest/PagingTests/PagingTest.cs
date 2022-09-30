using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EfCoreTest.Paging;
using Microsoft.EntityFrameworkCore;

namespace EfCoreTest;

internal class PagingTest : DbContextTest
{
    public static async Task Test()
    {
        var dbContext = CreatePostgreSqlDbContext();

        int pageIndex = 1;
        int pageSize = 10;

        var total = await dbContext.FactSales.AsNoTracking().CountAsync();

        // ReSharper disable once UselessBinaryOperation
        var skip = (pageIndex - 1) * pageSize;

        if (skip < total)
        {
            var page = await dbContext.FactSales.AsNoTracking().Skip(skip).Take(pageSize).ToListAsync();
        }
        else
        {
            var page = new List<FactSale>(0);
        }
    }

    public static async Task MsSql_Test()
    {
        var dbContext = CreateMsSqlDbContext();

        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

        var count = await dbContext.FactSales.AsNoTracking().CountAsync();

        var f = await dbContext.FactSales.AsNoTracking().Skip(count - 1).Take(1).ToListAsync();

        //200w 0.4s
        //exec sp_executesql N'SELECT [f].[date_id], [f].[other_data], [f].[product_id], [f].[quantity], [f].[store_id], [f].[unit_price]
        //FROM[fact_sales] AS[f]
        //ORDER BY(SELECT 1)
        //OFFSET @__p_0 ROWS FETCH NEXT @__p_1 ROWS ONLY',N'@__p_0 int, @__p_1 int',@__p_0=1999999,@__p_1=1
    }

    public static async Task PostgreSql_Test()
    {
        var dbContext = CreatePostgreSqlDbContext();

        //dbContext.Database.EnsureDeleted();
        //dbContext.Database.EnsureCreated();

        var count = await dbContext.FactSales.AsNoTracking().CountAsync();

        var f = await dbContext.FactSales.AsNoTracking().Skip(count - 1).Take(1).ToListAsync();

        //200w 2.7s
        //SELECT f.date_id, f.other_data, f.product_id, f.quantity, f.store_id, f.unit_price
        //FROM fact_sales AS f
        //LIMIT $1 OFFSET $2;
    }
}
