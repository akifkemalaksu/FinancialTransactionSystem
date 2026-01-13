using MassTransit;
using Messaging.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.IoC;
using TransactionService.Application.Features.TransferFeatures.CreateTransfer;
using TransactionService.Application.Services.Clients;
using TransactionService.Application.Services.DataAccessors;
using TransactionService.Infrastructure.Data;
using TransactionService.Infrastructure.Services.Clients;
using TransactionService.Infrastructure.Services.DataAccessors;

namespace TransactionService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddNpgsql<TransactionDbContext>(configuration.GetConnectionString("DatabaseConnection"));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMessagingBus<TransactionDbContext>(configuration, config =>
            {
                config.UsePostgres();
            });

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IFraudDetectionService, FraudDetectionService>();

            services.AddHttpClient(nameof(AccountService), conf =>
            {
                conf.BaseAddress = new Uri(configuration["Services:AccountService:BaseUrl"]!);
            });
            services.AddHttpClient(nameof(FraudDetectionService), conf =>
            {
                conf.BaseAddress = new Uri(configuration[$"Services:FraudDetectionService:BaseUrl"]!);
            });

            CQRSServiceRegistrar.Register(services, typeof(CreateTransferCommand));
        }
    }
}
