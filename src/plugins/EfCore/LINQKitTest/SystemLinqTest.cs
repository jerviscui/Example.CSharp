namespace LINQKitTest;

internal sealed class SystemLinqTest
{
    private readonly TestDbContext _dbContext;

    public SystemLinqTest(TestDbContext dbContext) => _dbContext = dbContext;

    public void Queryable_Default_Test()
    {
        //System.Linq.Queryable
        var query = _dbContext.Orders.Where(o => o.Id > 1);

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Where(o => (o.Id > 1))
    }

    public static void Queryable_Index_Test()
    {
        //System.Linq.Queryable

        //var query = _dbContext.Orders.Where((o, i) => o.Id > 1 && i != 2);
        //throw exception: System.InvalidOperationException
        //could not be translated
        //虽然 Queryable.Where 定义了带索引的参数 Expression<Func<TSource, int, bool>>，EntityFrameworkCore provider 没有对该方法做实现

        int[] numbers = { 0, 30, 20, 15, 90, 85, 40, 75 };

        var query = numbers.AsQueryable()
            .Where((o, i) => o > 1 && o != i);

        Console.WriteLine(query.Expression);
        //System.Int32[].Where((o, i) => ((o > 1) AndAlso (o != i)))

        var orders = query.ToList();
    }

    public void Enumerable_Func_Test()
    {
        bool Greater(Order o)
        {
            return o.Id > 1;
        }

        //System.Linq.Enumerable
        var query = _dbContext.Orders.AsQueryable()
            .Where(Greater); //虽然对 Queryable 调用 Enumerable.Where 重载方法，但是不会生成sql，Greater 方法在数据库返回结果后客户端调用生效
        var orders = query.ToList();

        //create sql:
        //SELECT [o].[Id], [o].[Amount], [o].[OrderDate]
        //FROM [Orders] AS[o]
    }
}
