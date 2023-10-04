using System.Linq.Expressions;
using LinqKit;

namespace LINQKitTest;

internal class CombiningTest
{
    private readonly TestDbContext _dbContext;

    public CombiningTest(TestDbContext dbContext) => _dbContext = dbContext;

    public void Combine_Test()
    {
        Expression<Func<OrderDetail, bool>> exp = p => p.Goods == "abc";
        Expression<Func<OrderDetail, bool>> exp2 = p => exp.Invoke(p) || p.Goods == "efg";

        var query = _dbContext.OrderDetails.AsExpandable().Where(exp2);
        //var query = _dbContext.OrderDetails.Where(exp2.Expand());//AsExpandable() works on IQueryable and Expand() works on Expression

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Where(p => ((p.Goods == "abc") OrElse (p.Goods == "efg")))

        var orders = query.ToList();
        //SELECT [o].[Id], [o].[Goods], [o].[OrderId]
        //FROM [OrderDetails] AS[o]
        //WHERE [o].[Goods] IN(N'abc', N'efg')
    }

    public void PredicateBuilder_Test()
    {
        var inner = PredicateBuilder.New<OrderDetail>();
        inner = inner.Or(NameFilter("abc"));
        inner = inner.Or(NameFilter("efg"));

        var outer = PredicateBuilder.New<OrderDetail>();
        outer = outer.And(IdFilter());
        outer = outer.And(inner);

        var query = _dbContext.OrderDetails.AsExpandable().Where(outer);

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Where(p => ((p.Id > 1) AndAlso((p.Goods == value(LINQKitTest.CombiningTest +<> c__DisplayClass4_0).name) OrElse(p.Goods == value(LINQKitTest.CombiningTest +<> c__DisplayClass4_0).name))))

        var orders = query.ToList();
        //SELECT [o].[Id], [o].[Goods], [o].[OrderId]
        //FROM [OrderDetails] AS [o]
        //WHERE ([o].[Id] > 1) AND (([o].[Goods] = @__name_0) OR ([o].[Goods] = @__name_1))
    }

    private static Expression<Func<OrderDetail, bool>> NameFilter(string name) => p => p.Goods == name;

    private static Expression<Func<OrderDetail, bool>> IdFilter() => p => p.Id > 1;
}
