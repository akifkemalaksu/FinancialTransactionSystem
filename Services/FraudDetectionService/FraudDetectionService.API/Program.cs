using FraudDetectionService.Application;
using ServiceDefaults.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.RegisterApplicationServices();
builder.Services.AddDefaultRateLimiting();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseExceptionHandling();
app.UseRequestResponseLogging();

app.UseRateLimiter();

app.UseAuthorization();

app.MapControllers();

app.Run();
