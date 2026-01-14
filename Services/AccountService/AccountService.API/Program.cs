using AccountService.Infrastructure;
using AccountService.Infrastructure.Extensions;
using ServiceDefaults.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterInfrastructureServices();

builder.Services.AddDefaultRateLimiting();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

app.Services.ApplyMigrations();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
