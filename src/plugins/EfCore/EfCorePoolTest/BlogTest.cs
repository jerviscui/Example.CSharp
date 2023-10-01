using EfCoreTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EfCorePoolTest;

internal class BlogTest
{
    private IServiceProvider ServiceProvider { get; set; }

    public BlogTest()
    {
        ServiceProvider = DependencyInjection.ServiceProvider;
    }

    public async Task Add_Test()
    {
        using var scope = ServiceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();

        await dbContext.Blogs.AddAsync(new Blog("add1", "test"));
        await dbContext.Blogs.AddAsync(new Blog("add2", "test"));

        await dbContext.SaveChangesAsync();
    }

    public async Task Add_Thrice_Test()
    {
        var t1 = Add_Test();
        var t2 = Add_Test();

        await Task.Delay(1_000);

        var t3 = Add_Test();

        await Task.WhenAll(t1, t2, t3);
    }

    public async Task Select_Test()
    {
        using var scope = ServiceProvider.CreateScope();
        await using var dbContext = scope.ServiceProvider.GetRequiredService<TestDbContext>();

        var list = await dbContext.Blogs.ToListAsync();
    }

    public async Task IDbContextFactory_Test()
    {
        var t1 = AddWithFactory();
        var t2 = AddWithFactory();

        await Task.Delay(1_000);

        var t3 = AddWithFactory();

        await Task.WhenAll(t1, t2, t3);
    }

    private async Task AddWithFactory()
    {
        await using var dbContext = await ServiceProvider.GetRequiredService<IDbContextFactory<TestDbContext>>()
            .CreateDbContextAsync();

        await dbContext.Blogs.AddAsync(new Blog("add3", "test"));

        await dbContext.SaveChangesAsync();
    }
}
