using AccountService.Application;
using AccountService.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.RegisterApplicationServices();
builder.Services.RegisterInfrastructureServices(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
