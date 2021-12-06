using Microsoft.EntityFrameworkCore;
using MiniprofilerTest.MVC;
using StackExchange.Profiling;
using StackExchange.Profiling.SqlFormatters;
using StackExchange.Profiling.Storage;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
// Add services to the container.
services.AddRazorPages();

services.AddMiniProfiler(options =>
{
    options.EnableDebugMode = true;

    options.RouteBasePath = "/profiler";
    options.SqlFormatter = new VerboseSqlServerFormatter(true);

    //options.Storage =
    //    new RedisStorage(builder.Configuration.GetConnectionString("MsSql"))
    //    {
    //        CacheDuration = TimeSpan.FromMinutes(30)
    //    };

    options.Storage = new SqlServerStorage(builder.Configuration.GetConnectionString("MsSql"));

    //((DatabaseStorageBase)options.Storage).TableCreationScripts;
}).AddEntityFramework();

services.AddDbContextPool<TestDbContext>(optionsBuilder =>
{
    optionsBuilder.UseSqlServer(builder.Configuration.GetConnectionString("MsSql"));
});
services.AddDbContextFactory<TestDbContext>();

var app = builder.Build();

//Create MiniProfiler Schema
app.Lifetime.ApplicationStarted.Register(() =>
{
    if (MiniProfiler.DefaultOptions.Storage is DatabaseStorageBase storage)
    {
        using var dbContext = app.Services.GetRequiredService<IDbContextFactory<TestDbContext>>().CreateDbContext();

        foreach (var script in storage.TableCreationScripts)
        {
            dbContext.Database.ExecuteSqlRaw(script);
        }
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseMiniProfiler();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
