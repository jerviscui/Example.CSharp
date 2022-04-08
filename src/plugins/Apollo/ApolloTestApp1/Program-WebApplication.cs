using System.Runtime.InteropServices;
using System.Text;
using ApolloTestApp1;
using Com.Ctrip.Framework.Apollo;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var idc = Environment.GetEnvironmentVariable("IDC");
if (idc == string.Empty)
{
    idc = null;
}
if (idc is null)
{
    //read file
    string? path = null;
    if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
    {
        path = @"C:\opt\settings\server.properties";
    }
    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
    {
        path = "/opt/settings/server.properties";
    }
    if (path is not null)
    {
        try
        {
            var strings = File.ReadAllLines(path, Encoding.UTF8);
            var dictionary = strings.Select(o => o.Split('='))
                .ToDictionary(o => o[0], o => o[1]);

            dictionary.TryGetValue("idc", out idc);
        }
        catch (DirectoryNotFoundException)
        {
        }
        catch (FileNotFoundException)
        {
        }
    }
}

var options = builder.Configuration.GetSection("apollo").Get<ApolloOptions>();
options.DataCenter = idc;
var apollo = builder.Configuration.AddApollo(options);

var config = builder.Configuration.GetSection("MyOption");
var name = config.GetSection("Name");
var ints = config.GetSection("Ints").Get<int[]>();
builder.Services.Configure<MyOption>(config);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapDefaultControllerRoute();

app.Run();
