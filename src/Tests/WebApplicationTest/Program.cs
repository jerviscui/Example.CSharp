using Scalar.AspNetCore;

namespace WebApplicationTest;

public static class Program
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
        _ = services.AddOpenApi();

        _ = services.AddHttpContextAccessor();

        _ = services.Configure<SimpleOptions>(builder.Configuration.GetSection("SimpleOptions"));

        _ = services.Configure<ReadOnlyOptions>(
            builder.Configuration.GetSection("ReadOnlyOptions"),
            options => options.BindNonPublicProperties = true);

        _ = services.Configure<PrivateOptions>(
            builder.Configuration.GetSection("PrivateOptions"),
            options => options.BindNonPublicProperties = true);

        _ = builder.WebHost
            .ConfigureKestrel(
                serverOptions =>
                {
                    serverOptions.Limits.MaxResponseBufferSize = 10; // Disable response buffering limit
                });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.MapOpenApi();
            _ = app.MapScalarApiReference();
        }

        //_ = app.UseHttpsRedirection();

        _ = app.UseAuthorization();

        _ = app.MapControllers();

        app.Run();
    }

    #endregion

}
