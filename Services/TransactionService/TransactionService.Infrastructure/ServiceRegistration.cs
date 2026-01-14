using Messaging.Persistence.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.CircuitBreaker;
using Polly.Retry;
using ServiceDefaults.Extensions;
using TransactionService.Application.Features.TransferFeatures.CreateTransfer;
using TransactionService.Application.Services.Clients;
using TransactionService.Application.Services.DataAccessors;
using TransactionService.Infrastructure.BackgroundServices;
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

            builder.Services.AddScoped<IAccountApiService, AccountApiService>();
            builder.Services.AddScoped<IFraudDetectionApiService, FraudDetectionApiService>();
            builder.Services.AddScoped<ITransferApiService, MockTransferApiService>();
            builder.Services.AddHostedService<TransferStatusWorker>();

            builder.Services.AddHttpClient(nameof(AccountApiService), conf =>
            {
                conf.BaseAddress = new Uri(builder.Configuration["Services:AccountApiService:BaseUrl"]!);
            })
            .AddResilienceHandler("account-api-resilience", pipeline =>
            {
                pipeline.AddRetry(GetRetryOptions());
                pipeline.AddCircuitBreaker(GetCircuitBreakerOptions());
                pipeline.AddTimeout(TimeSpan.FromSeconds(5));
            });

            builder.Services.AddHttpClient(nameof(FraudDetectionApiService), conf =>
            {
                conf.BaseAddress = new Uri(builder.Configuration[$"Services:FraudDetectionApiService:BaseUrl"]!);
            })
            .AddResilienceHandler("fraud-api-resilience", pipeline =>
            {
                pipeline.AddRetry(GetRetryOptions());
                pipeline.AddCircuitBreaker(GetCircuitBreakerOptions());
                pipeline.AddTimeout(TimeSpan.FromSeconds(5));
            });

            builder.Services.RegisterCQRSServices(typeof(CreateTransferCommand));

            builder.ConfigureOpenTelemetry();
        }

        private static RetryStrategyOptions<HttpResponseMessage> GetRetryOptions()
        {
            return new RetryStrategyOptions<HttpResponseMessage>
            {
                MaxRetryAttempts = 3,
                Delay = TimeSpan.FromSeconds(2),
                BackoffType = DelayBackoffType.Exponential,
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .HandleResult(response => !response.IsSuccessStatusCode)
            };
        }

        private static CircuitBreakerStrategyOptions<HttpResponseMessage> GetCircuitBreakerOptions()
        {
            return new CircuitBreakerStrategyOptions<HttpResponseMessage>
            {
                SamplingDuration = TimeSpan.FromSeconds(10),
                FailureRatio = 0.2,
                MinimumThroughput = 3,
                BreakDuration = TimeSpan.FromSeconds(30),
                ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
                    .Handle<HttpRequestException>()
                    .HandleResult(response => !response.IsSuccessStatusCode)
            };
        }
    }
}
