using AccountService.Application.Features.AccountFeatures.GetAccountById;
using AccountService.Application.Features.AccountFeatures.UpdateAccountBalance;
using AccountService.Application.Services.DataAccessors;
using AccountService.Application.Services.InfrastructureServices;
using AccountService.Infrastructure.Data;
using AccountService.Infrastructure.Services.DataAccessors;
using AccountService.Infrastructure.Services.InfrastructureServices;
using MassTransit;
using Messaging.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.IoC;
using StackExchange.Redis;

namespace AccountService.Infrastructure
{
    public static class ServiceRegistration
    {
        public static void RegisterInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            services.AddNpgsql<AccountDbContext>(configuration.GetConnectionString("DatabaseConnection"));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddSingleton<IConnectionMultiplexer>(sp => ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection")));
            services.AddScoped<IDistributedCacheService, RedisCacheService>();

            services.AddMessagingBus<AccountDbContext>(configuration, config => {
                config.UsePostgres();
            });

            CQRSServiceRegistrar.Register(services, typeof(GetAccountByIdQuery));
        }
    }
}