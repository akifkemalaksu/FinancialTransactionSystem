using Microsoft.Extensions.DependencyInjection;

namespace TransactionService.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            // İleride CQRS handler'ları buraya eklenebilir
            return services;
        }
    }
}
