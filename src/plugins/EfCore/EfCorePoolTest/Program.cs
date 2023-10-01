using EfCorePoolTest;
using EfCoreTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddDbContextPool<TestDbContext>(SettingBuilder, 2);
services.AddPooledDbContextFactory<TestDbContext>(SettingBuilder, 2);

DependencyInjection.ServiceProvider = services.BuildServiceProvider();

var blogTest = new BlogTest();

//await blogTest.Add_Test();

await blogTest.Add_Thrice_Test();

await blogTest.Select_Test();

await blogTest.IDbContextFactory_Test();

return;

static void SettingBuilder(DbContextOptionsBuilder builder)
{
    DbContextTest.SettingBuilder(builder);
    DbContextTest.UseSqlServer(builder);
}
