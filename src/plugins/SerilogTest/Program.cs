                                                                                                                                                                                                                               using Microsoft.AspNetCore.HttpLogging;
using Serilog;
using Serilog.Core;
using Serilog.Exceptions;
using Serilog.Exceptions.Core;
using Serilog.Exceptions.EntityFrameworkCore.Destructurers;

namespace SerilogTest;

internal class Program
{

    #region Constants & Statics

    private static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .AddJsonFile(
                $"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json",
                true)
            .Build();

        var loggerConfiguration = new LoggerConfiguration()
            .ReadFrom
            .Configuration(configuration);
        Log.Logger = ConfigureLogger(loggerConfiguration).CreateBootstrapLogger();

        try
        {
            Log.Information("Starting web application");

#pragma warning disable CA2201 // Do not raise reserved exception types
            Log.Error(new MyException("Host error.", new Exception("inner exception")), "test exception");
#pragma warning restore CA2201 // Do not raise reserved exception types

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            _ = builder.Host
                .UseSerilog(
                    (builderContext, serviceProvider, options) =>
                    {
                        _ = options.ReadFrom.Configuration(builderContext.Configuration).ReadFrom
                            .Services(serviceProvider);
                        _ = ConfigureLogger(options);
                    });

            var services = builder.Services;
            _ = services.AddControllers();
            _ = services.AddHttpContextAccessor();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            _ = services.AddOpenApi();

            _ = services
                .AddHttpLogging(
                    (options) =>
                    {
                        options.LoggingFields = HttpLoggingFields.All;
                    });

            _ = services.AddHealthChecks();

            var app = builder.Build();

            _ = app.UseForwardedHeaders();
            _ = app.UseHttpsRedirection();

            _ = app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                _ = app.MapOpenApi();
            }

            if (!app.Environment.IsDevelopment())
            {
                _ = app.UseHsts();
            }

            _ = app.UseHttpLogging();

            _ = app.UseHealthChecks("/health");

            //_ = app.UseSerilogRequestLogging(
            //    (options) =>
            //    {
            //        // Attach additional properties to the request completion event
            //        options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            //        {
            //            diagnosticContext.Set("RequestHost", httpContext.Request.Host);
            //            diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
            //        };
            //    });

            _ = app.UseAuthorization();

            _ = app.MapControllers();

            await app.RunAsync();

            Log.Information("Stoped web application");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }

        static LoggerConfiguration ConfigureLogger(LoggerConfiguration configuration)
        {
            return configuration.Enrich
                .WithProperty(Constants.SourceContextPropertyName, typeof(Program).FullName, false)
                .Enrich
                .WithExceptionDetails(
                    new DestructuringOptionsBuilder().WithDefaultDestructurers()
                        .WithIgnoreStackTraceAndTargetSiteExceptionFilter()
                        .WithDestructurers([new DbUpdateExceptionDestructurer()]));
        }
    }

    #endregion

    protected Program()
    {
    }
}
