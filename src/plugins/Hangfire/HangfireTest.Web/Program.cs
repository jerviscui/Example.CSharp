using Hangfire;
using Hangfire.Dashboard;
using Hangfire.Redis.StackExchange;
using HangfireTest.Service;

namespace HangfireTest.Web;

internal static class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        var services = builder.Services;
        _ = services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        _ = services.AddEndpointsApiExplorer();
        _ = services.AddSwaggerGen();

        _ = services.AddTestJobs();
        _ = services.AddSingleton<ExternalDataFilterAttribute>();

        _ = services.AddHangfire(
            (serviceProvider, configuration) =>
            {
                _ = configuration.UseRedisStorage("127.0.0.1:6379,DefaultDatabase=7,allowAdmin=true");

                var filter = serviceProvider.GetRequiredService<ExternalDataFilterAttribute>();
                _ = configuration.UseFilter(filter);

                _ = configuration.UseDashboardMetrics(DashboardMetrics.GetMetrics().ToArray());

                //_ = configuration.UseDashboardMetric(
                //    RedisStorage.GetDashboardMetricFromRedisInfo("使用内存", RedisInfoKeys.used_memory_human));
                //_ = configuration.UseDashboardMetric(
                //    RedisStorage.GetDashboardMetricFromRedisInfo("高峰内存", RedisInfoKeys.used_memory_peak_human));
            });

        _ = services.AddHangfireServer(
            options =>
            {
                options.ServerName = "HangfireTest.Web";
                options.Queues = ["default"];
            });

        var app = builder.Build();

        var filter = app.Services.GetRequiredService<ExternalDataFilterAttribute>();
        _ = app.Lifetime.ApplicationStarted
            .Register(
                () =>
                {
                    GlobalJobFilters.Filters.Add(filter);
                },
                true);

        var options = new DashboardOptions
        {
            Authorization = [new AllowAnonymousAuthorizationFilter()],
            IgnoreAntiforgeryToken = true
        };
        if (app.Environment.IsProduction())
        {
            options.DisplayStorageConnectionString = false;
        }
        _ = app.UseHangfireDashboard("/hangfire", options);

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.UseSwagger();
            _ = app.UseSwaggerUI();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseAuthorization();

        _ = app.MapControllers();

        app.Run();
    }

    #endregion

}
