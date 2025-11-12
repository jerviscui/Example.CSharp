using EfCoreTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Runtime.InteropServices;

namespace EfCorePoolTest;

public sealed class BlogTest
{
    public BlogTest()
    {
        ServiceProvider = DependencyInjection.ServiceProvider;
    }

    #region Properties

    private IServiceProvider ServiceProvider { get; set; }

    #endregion

    #region Methods

    private async Task AddByScopeAsync(long id, CancellationToken cancellationToken)
    {
        using var scope = ServiceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();
        Console.WriteLine(dbContext.Id);

        _ = await dbContext.Blogs.AddAsync(new Blog(id, "add1", "test"), cancellationToken);

        await Task.Delay(100, cancellationToken);

        _ = await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task AddWithFactoryAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await ServiceProvider.GetRequiredService<IDbContextFactory<TestDbContext>>()
            .CreateDbContextAsync(cancellationToken);

        _ = await dbContext.Blogs.AddAsync(new Blog(1003, "add3", "test"), cancellationToken);

        _ = await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task Add_TestAsync(CancellationToken cancellationToken = default)
    {
        await AddByScopeAsync(1001, cancellationToken);
    }

    public async Task Add_Thrice_TestAsync(CancellationToken cancellationToken = default)
    {
        // pool size 2
        var t1 = AddByScopeAsync(2001, cancellationToken);
        var t2 = AddByScopeAsync(2002, cancellationToken);
        var t3 = AddByScopeAsync(2003, cancellationToken);

        await Task.Delay(1_000, cancellationToken);

        var t4 = AddByScopeAsync(2004, cancellationToken);
        var t5 = AddByScopeAsync(2005, cancellationToken);

        await Task.WhenAll(t1, t2, t3, t4);
    }

    public async Task IDbContextFactory_TestAsync(CancellationToken cancellationToken = default)
    {
        var t1 = AddWithFactoryAsync(cancellationToken);
        var t2 = AddWithFactoryAsync(cancellationToken);

        await Task.Delay(1_000, default);

        var t3 = AddWithFactoryAsync(cancellationToken);

        await Task.WhenAll(t1, t2, t3);
    }

    public unsafe void ScopeInstance_Test()
    {
        using var scope = ServiceProvider.CreateScope();
        var dbContext1 = scope.ServiceProvider.GetRequiredService<TestDbContext>();
        var dbContext2 = scope.ServiceProvider.GetRequiredService<TestDbContext>();

        var handle1 = GCHandle.Alloc(dbContext1, GCHandleType.Normal);
        var handle2 = GCHandle.Alloc(dbContext2, GCHandleType.Normal);
        var ptr1 = GCHandle.ToIntPtr(handle1);
        var ptr2 = GCHandle.ToIntPtr(handle2);

        // the variables location
        Console.WriteLine(ptr1);
        Console.WriteLine(ptr2);

        handle2.Free();
        handle1.Free();

        // the instance location in heap
        fixed (long* p = &dbContext1.Id)
        {
            Console.WriteLine(*p);
        }
        fixed (long* p = &dbContext2.Id)
        {
            Console.WriteLine(*p);
        }

        Console.WriteLine(ReferenceEquals(dbContext1, dbContext2));
    }

    public async Task Select_TestAsync(CancellationToken cancellationToken = default)
    {
        using var scope = ServiceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();

        var list = await dbContext.Blogs.ToListAsync(cancellationToken: cancellationToken);
    }

    #endregion

}
