using TransactionService.API.Extensions;
using TransactionService.Infrastructure;
using TransactionService.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterInfrastructureServices();

builder.Services.AddTransactionRateLimiting();

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
