using AccountService.Application.Features.AccountFeatures.GetAccountById;
using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.IoC;

namespace AccountService.Application
{
    public static class ServiceRegistration
    {
        public static IServiceCollection RegisterApplicationServices(this IServiceCollection services)
        {
            CQRSServiceRegistrar.Register(services, typeof(GetAccountByIdQuery));

            return services;
        }
    }
}
