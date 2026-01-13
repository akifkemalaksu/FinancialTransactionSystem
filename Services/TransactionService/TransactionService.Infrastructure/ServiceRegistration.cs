using Messaging.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.Extensions;
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
        public static void RegisterInfrastructureServices(this WebApplicationBuilder builder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddNpgsql<TransactionDbContext>(builder.Configuration.GetConnectionString("DatabaseConnection"));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.AddMessagingBus<TransactionDbContext>(typeof(CreateTransferCommand));

            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IFraudDetectionService, FraudDetectionService>();

            builder.Services.AddHttpClient(nameof(AccountService), conf =>
            {
                conf.BaseAddress = new Uri(builder.Configuration["Services:AccountService:BaseUrl"]!);
            });
            builder.Services.AddHttpClient(nameof(FraudDetectionService), conf =>
            {
                conf.BaseAddress = new Uri(builder.Configuration[$"Services:FraudDetectionService:BaseUrl"]!);
            });

            CQRSServiceRegistrar.Register(builder.Services, typeof(CreateTransferCommand));
        }
    }
}
