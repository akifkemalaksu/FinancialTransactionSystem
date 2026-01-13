using AccountService.Application.EventHandlers;
using AccountService.Application.Features.AccountFeatures.GetAccountById;
using AccountService.Application.Services.DataAccessors;
using AccountService.Application.Services.InfrastructureServices;
using AccountService.Infrastructure.Data;
using AccountService.Infrastructure.Services.DataAccessors;
using AccountService.Infrastructure.Services.InfrastructureServices;
using Messaging.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.Extensions;
using StackExchange.Redis;

namespace AccountService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this WebApplicationBuilder builder)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            builder.Services.AddNpgsql<AccountDbContext>(builder.Configuration.GetConnectionString("DatabaseConnection"));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(builder.Configuration.GetConnectionString("RedisConnection")));
            builder.Services.AddScoped<IDistributedCacheService, RedisCacheService>();

            builder.AddMessagingBus<AccountDbContext>(typeof(TransferCreatedEventHandler));

            CQRSServiceRegistrar.Register(builder.Services, typeof(GetAccountByIdQuery));
        }
    }
}