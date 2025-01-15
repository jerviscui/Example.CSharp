using System.Linq.Expressions;
using LinqKit;

namespace LINQKitTest;

internal sealed class NavigationFilterTest
{
    private readonly TestDbContext _dbContext;

    public NavigationFilterTest(TestDbContext dbContext) => _dbContext = dbContext;

    public void Navigation_Func_Test()
    {
        //System.Linq.Enumerable
        var query = _dbContext.Orders.Where(o => o.Id > 1 && o.Details.Any(p => p.Goods == "abc"));

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Where(o => ((o.Id > 1) AndAlso o.Details.Any(p => (p.Goods == "abc"))))

        var orders = query.ToList();
        //create sql:
        //SELECT [o].[Id], [o].[Amount], [o].[OrderDate]
        //FROM [Orders] AS[o]
        //WHERE ([o].[Id] > 1) AND EXISTS(
        //    SELECT 1
        //    FROM [OrderDetails] AS[o0]
        //    WHERE ([o].[Id] = [o0].[OrderId]) AND([o0].[Goods] = N'abc'))
    }

    public void Navigation_Expression_Failed_Test()
    {
        Expression<Func<OrderDetail, bool>> exp = p => p.Goods == "abc";

        var query = _dbContext.Orders.Where(o => o.Details.Any(exp.Compile()));

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Where(o => o.Details.Any(value(LINQKitTest.NavigationFilterTest+<>c__DisplayClass3_0).exp.Compile()))

        Console.WriteLine(query.Expression.Expand());
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Where(o => o.Details.Any(p => (p.Goods == "abc")))

        var orders = query.ToList();
        //throw exception: System.ArgumentException
        //Expression of type cannot be used for parameter of type
    }

    public void Navigation_Expression_AsExpandable_Test()
    {
        Expression<Func<OrderDetail, bool>> exp = p => p.Goods == "abc";

        var query = _dbContext.Orders.AsExpandable().Where(o => o.Details.Any(exp.Compile()));

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Where(o => o.Details.Any(p => (p.Goods == "abc")))

        var orders = query.ToList();
        //SELECT [o].[Id], [o].[Amount], [o].[OrderDate]
        //FROM [Orders] AS[o]
        //WHERE EXISTS(
        //    SELECT 1
        //    FROM [OrderDetails] AS[o0]
        //    WHERE ([o].[Id] = [o0].[OrderId]) AND([o0].[Goods] = N'abc'))
    }

    public void Navigation_Expression_Expand_Test()
    {
        Expression<Func<OrderDetail, bool>> exp = p => p.Goods == "abc";
        Expression<Func<Order, bool>> expand = o => o.Details.Any(exp.Compile());

        Console.WriteLine(expand);
        //o => o.Details.Any(value(LINQKitTest.NavigationFilterTest +<> c__DisplayClass5_0).exp.Compile())
        Console.WriteLine(expand.Expand());
        //o => o.Details.Any(p => (p.Goods == "abc"))

        var query = _dbContext.Orders.Where(expand.Expand());

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Where(o => o.Details.Any(p => (p.Goods == "abc")))
    }

    public void Subqueries_Expression_Test()
    {
        var query = from o in _dbContext.Orders
            let details = _dbContext.OrderDetails.Where(d => d.OrderId == o.Id).ToList()
            where details.Any(p => p.Goods == "abc")
            select o;

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Select(o => new <>f__AnonymousType0`2(o = o, details = value(LINQKitTest.NavigationFilterTest)._dbContext.OrderDetails.Where(d => (d.OrderId == o.Id)).ToList())).Where(<>h__TransparentIdentifier0 => <>h__TransparentIdentifier0.details.Any(p => (p.Goods == "abc"))).Select(<>h__TransparentIdentifier0 => <>h__TransparentIdentifier0.o)

        var orders = query.ToList();
        //SELECT [o].[Id], [o].[Amount], [o].[OrderDate]
        //FROM [Orders] AS[o]
        //WHERE EXISTS(
        //    SELECT 1
        //    FROM [OrderDetails] AS[o0]
        //    WHERE ([o].[Id] = [o0].[OrderId]) AND([o0].[Goods] = N'abc'))
    }

    public void Subqueries_Expression_AsExpandable_Test()
    {
        Expression<Func<OrderDetail, bool>> exp = p => p.Goods == "abc";

        var query = from o in _dbContext.Orders.AsExpandable()
            let details = _dbContext.OrderDetails.Where(d => d.OrderId == o.Id).ToList()
            where details.Any(exp.Compile())
            select o;

        Console.WriteLine(query.Expression);
        //[Microsoft.EntityFrameworkCore.Query.QueryRootExpression].Select(o => new <> f__AnonymousType0`2(o = o, details = value(LINQKitTest.NavigationFilterTest)._dbContext.OrderDetails.Where(d => (d.OrderId == o.Id)).ToList())).Where(<> h__TransparentIdentifier0 => <> h__TransparentIdentifier0.details.Any(p => (p.Goods == "abc"))).Select(<> h__TransparentIdentifier0 => <> h__TransparentIdentifier0.o)

        var orders = query.ToList();
        //SELECT [o].[Id], [o].[Amount], [o].[OrderDate]
        //FROM [Orders] AS[o]
        //WHERE EXISTS(
        //    SELECT 1
        //    FROM [OrderDetails] AS[o0]
        //    WHERE ([o].[Id] = [o0].[OrderId]) AND([o0].[Goods] = N'abc'))
    }
}
