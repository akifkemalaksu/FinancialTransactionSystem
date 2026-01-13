using Microsoft.Extensions.DependencyInjection;
using ServiceDefaults.Implementations;
using ServiceDefaults.Interfaces;
using System.Reflection;

namespace ServiceDefaults.Extensions
{
    public static class CQRSServiceRegistrar
    {
        public static IServiceCollection Register(this IServiceCollection services, Type type)
        {
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();
            services.AddScoped<ICommandDispatcher, CommandDispatcher>();

            services.Scan(selector =>
            {
                selector.FromAssembliesOf(type)
                        .AddClasses(filter =>
                        {
                            filter.AssignableTo(typeof(IQueryHandler<,>));
                        })
                        .AsImplementedInterfaces()
                        .WithScopedLifetime();
            });
            services.Scan(selector =>
            {
                selector.FromAssembliesOf(type)
                        .AddClasses(filter =>
                        {
                            filter.AssignableTo(typeof(ICommandHandler<,>));
                        })
                        .AsImplementedInterfaces()
                        .WithScopedLifetime();
            });

            return services;
        }
    }
}
