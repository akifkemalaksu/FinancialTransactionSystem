using LedgerService.Infrastructure.Data;
using MassTransit;
using Messaging.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LedgerService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddNpgsql<LedgerDbContext>(configuration.GetConnectionString("DatabaseConnection"));

            services.AddMessagingBus<LedgerDbContext>(configuration, config => {
                config.UsePostgres();
            });
        }
    }
}
