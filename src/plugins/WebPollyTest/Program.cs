using Microsoft.AspNetCore.HttpLogging;

namespace WebPollyTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        _ = builder.Logging.AddConsole();

        _ = builder.Services.AddControllers();
        _ = builder.Services.AddOpenApi();

        _ = builder.Services
            .AddHttpLogging(
                (options) =>
                {
                    options.LoggingFields = HttpLoggingFields.All;
                    options.CombineLogs = true;
                });

        var app = builder.Build();

        _ = app.UseHttpLogging();

        if (app.Environment.IsDevelopment())
        {
            _ = app.MapOpenApi();
        }

        //_ = app.UseHttpsRedirection();

        _ = app.UseAuthorization();

        _ = app.MapControllers();

        app.Run();
    }

    #endregion

}
