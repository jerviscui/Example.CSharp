using EfCoreTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EfCorePoolTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main()
    {
        var services = new ServiceCollection();

        _ = services.AddDbContextPool<TestDbContext>(SettingBuilder, 2);
        _ = services.AddPooledDbContextFactory<TestDbContext>(SettingBuilder, 2);

        DependencyInjection.ServiceProvider = services.BuildServiceProvider();

        var blogTest = new BlogTest();

        //await blogTest.Add_TestAsync(default);

        //await blogTest.Add_Thrice_TestAsync(default);

        blogTest.ScopeInstance_Test();

        //await blogTest.Select_TestAsync(default);

        //await blogTest.IDbContextFactory_TestAsync(default);

        return;

        static void SettingBuilder(DbContextOptionsBuilder builder)
        {
            DbContextTest.SettingBuilder(builder);
            DbContextTest.UseSqlServer(builder);
        }
    }

    #endregion

}
