using Com.Ctrip.Framework.Apollo.Logging;
using LogLevel = Com.Ctrip.Framework.Apollo.Logging.LogLevel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var apollo = builder.Configuration.GetSection("apollo");
builder.WebHost.ConfigureAppConfiguration((context, configBuilder) => configBuilder.AddApollo(apollo));

LogManager.UseConsoleLogging(LogLevel.Info);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
