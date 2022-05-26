using WebApplicationTest;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.Configure<SimpleOptions>(builder.Configuration.GetSection("SimpleOptions"));

services.Configure<ReadOnlyOptions>(builder.Configuration.GetSection("ReadOnlyOptions"),
    options => options.BindNonPublicProperties = true);

services.Configure<PrivateOptions>(builder.Configuration.GetSection("PrivateOptions"),
    options => options.BindNonPublicProperties = true);

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
