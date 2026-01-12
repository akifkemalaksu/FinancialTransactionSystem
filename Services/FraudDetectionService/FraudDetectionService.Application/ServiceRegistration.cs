using FraudDetectionService.Application.Features.FraudCheck;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.IoC;

namespace FraudDetectionService.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            CQRSServiceRegistrar.Register(services, typeof(CheckFraudQuery));
            return services;
        }
    }
}
