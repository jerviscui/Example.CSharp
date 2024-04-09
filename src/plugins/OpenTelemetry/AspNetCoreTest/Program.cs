using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetry().WithTracing(providerBuilder =>
{
    providerBuilder.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("AspNetCoreTest", "1.0.0"));

    providerBuilder.AddAspNetCoreInstrumentation(options =>
    {
        //options.Filte
    });

    providerBuilder.SetSampler(new AlwaysOnSampler());
    //providerBuilder.SetSampler(new ParentBasedSampler(new AlwaysOnSampler()));

    providerBuilder.AddZipkinExporter(options => { options.Endpoint = new Uri("http://localhost:9411/api/v2/spans"); });
    //providerBuilder.AddConsoleExporter();
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
