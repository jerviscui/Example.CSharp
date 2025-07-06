namespace WebPollyTest;

internal static class Program
{

    #region Constants & Statics

    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        _ = builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        _ = builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            _ = app.MapOpenApi();
        }

        _ = app.UseHttpsRedirection();

        _ = app.UseAuthorization();

        _ = app.MapControllers();

        app.Run();
    }

    #endregion

}