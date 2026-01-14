using Microsoft.AspNetCore.RateLimiting;
using ServiceDefaults.Extensions;
using System.Threading.RateLimiting;

namespace TransactionService.API.Extensions
{
    public static class RateLimitingExtensions
    {
        public static IServiceCollection AddTransactionRateLimiting(this IServiceCollection services)
        {
            services.AddDefaultRateLimiting();

            services.Configure<RateLimiterOptions>(options =>
            {
                options.AddPolicy("transaction-create", httpContext =>
                {
                    var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
                    return RateLimitPartition.GetTokenBucketLimiter(
                        ip,
                        _ => new TokenBucketRateLimiterOptions
                        {
                            TokenLimit = 20,
                            TokensPerPeriod = 5,
                            ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                            AutoReplenishment = true,
                            QueueLimit = 0,
                            QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                        });
                });
            });

            return services;
        }
    }
}
