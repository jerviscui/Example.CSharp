using Microsoft.AspNetCore.HttpLogging;

//ThreadPool.SetMinThreads(256, 256);
//ThreadPool.GetMaxThreads(out var c, out var c2);
//ThreadPool.GetMinThreads(out var m, out var m2);
//ThreadPool.GetAvailableThreads(out var a, out var a2);

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://*:10000", "https://*:10001");

// Add services to the container.
var services = builder.Services;
services.AddControllers();

services.AddHttpLogging(options =>
{
    // Customize HTTP logging.
    options.LoggingFields = HttpLoggingFields.All;
    options.RequestHeaders.Add("My-Request-Header");
    options.ResponseHeaders.Add("My-Response-Header");
    options.MediaTypeOptions.AddText("application/javascript");
    options.RequestBodyLogLimit = 4096;
    options.ResponseBodyLogLimit = 4096;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "MinimalApiTest.WebApi.xml"), true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpLogging();

//app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
    {
        var forecast = Enumerable.Range(1, 5).Select(index =>
                new WeatherForecast
                (
                    DateTime.Now.AddDays(index),
                    Random.Shared.Next(-20, 55),
                    summaries[Random.Shared.Next(summaries.Length)]
                ))
            .ToArray();
        return forecast;
    })
    .WithName("GetWeatherForecast");

app.MapDefaultControllerRoute();

app.Run();
