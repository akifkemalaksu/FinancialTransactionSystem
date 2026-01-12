using MassTransit;
using Messaging.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.Features.NotificationFeatures.CreateNotification;
using NotificationService.Application.Services.DataAccessors;
using NotificationService.Infrastructure.Data;
using NotificationService.Infrastructure.Services.DataAccessors;
using ServiceDefaults.IoC;

namespace NotificationService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddNpgsql<NotificationDbContext>(configuration.GetConnectionString("DatabaseConnection"));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMessagingBus<NotificationDbContext>(configuration, config => {
                config.UsePostgres();
            });

            CQRSServiceRegistrar.Register(services, typeof(CreateNotificationCommand));
        }
    }
}
