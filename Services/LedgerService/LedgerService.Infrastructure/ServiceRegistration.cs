using LedgerService.Application.EventHandlers;
using LedgerService.Application.Features.LedgerFeatures.CreateLedger;
using LedgerService.Application.Services.DataAccessors;
using LedgerService.Infrastructure.Data;
using LedgerService.Infrastructure.Services.DataAccessors;
using Messaging.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.Extensions;

namespace LedgerService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this WebApplicationBuilder builder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddNpgsql<LedgerDbContext>(builder.Configuration.GetConnectionString("DatabaseConnection"));

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.AddMessagingBus<LedgerDbContext>(typeof(TransferCompletedEventHandler));

            builder.Services.RegisterCQRSServices(typeof(CreateLedgerCommand));

            builder.ConfigureOpenTelemetry();
        }
    }
}
