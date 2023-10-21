using AuthorizationTest;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var services = builder.Services;
services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
services.AddEndpointsApiExplorer();
services.AddSwaggerGen();

services.AddSingleton<IAuthorizationHandler, MinimumAgeHandler>();

services.AddAuthentication()
    .AddScheme<AuthenticationSchemeOptions, TestAuthenticationHandler>(TestAuthenticationHandler.SchemeName, null)
    .AddScheme<AuthenticationSchemeOptions, MinimumAgeAuthenticationHandler>(MinimumAgeAuthenticationHandler.SchemeName,
        null);

services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
    options.AddPolicy("Test", policy => policy.RequireRole("roleA", "roleB").RequireUserName("test"));
    options.AddPolicy("MinimumAge", policy => policy.AddRequirements(new MinimumAgeRequirement(10)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
