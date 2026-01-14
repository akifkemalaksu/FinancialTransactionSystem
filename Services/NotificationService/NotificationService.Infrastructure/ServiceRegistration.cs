using Messaging.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NotificationService.Application.EventHandlers;
using NotificationService.Application.Features.NotificationFeatures.CreateNotification;
using NotificationService.Application.Services.DataAccessors;
using NotificationService.Infrastructure.Data;
using NotificationService.Infrastructure.Services.DataAccessors;
using ServiceDefaults.Extensions;

namespace NotificationService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this WebApplicationBuilder builder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddNpgsql<NotificationDbContext>(builder.Configuration.GetConnectionString("DatabaseConnection"));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.AddMessagingBus<NotificationDbContext>(typeof(TransferCreatedEventHandler));

            builder.Services.RegisterCQRSServices(typeof(CreateNotificationCommand));

            builder.ConfigureOpenTelemetry();
        }
    }
}
