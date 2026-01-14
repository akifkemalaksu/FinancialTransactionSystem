using Messaging.Abstractions;
using Messaging.Configurations;
using Messaging.Infrastructure;
using Messaging.Persistence.BackgroundServices;
using Messaging.Persistence.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace Messaging.Persistence.Extensions
{
    public static class MessagingRegistration
    {
        public static IHostApplicationBuilder AddMessagingBus<TDbContext>(
            this IHostApplicationBuilder builder,
            Type type
        )
        where TDbContext : DbContext
        {
            builder.Services.Configure<KafkaSettings>(builder.Configuration.GetSection(nameof(KafkaSettings)));

            builder.Services.AddScoped<DbContext>(sp => sp.GetRequiredService<TDbContext>());

            builder.Services.AddScoped<IKafkaProducer, OutboxProducer>();
            builder.Services.AddSingleton<IActualKafkaProducer, ActualKafkaProducer>();

            builder.Services.AddHostedService<OutboxProcessorWorker>();

            var assembly = Assembly.GetAssembly(type);
            var apiAssembly = Assembly.GetEntryAssembly();

            builder.Services.Scan(scan => scan
                .FromAssemblies(assembly)
                .AddClasses(classes => classes.AssignableTo(typeof(IKafkaHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

            var consumerTypes = apiAssembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && IsSubclassOfRawGeneric(typeof(KafkaConsumerBase<>), t));

            foreach (var consumerType in consumerTypes)
            {
                builder.Services.AddSingleton(typeof(IHostedService), consumerType);
            }

            return builder;
        }

        private static bool IsSubclassOfRawGeneric(Type generic, Type toCheck)
        {
            while (toCheck != null && toCheck != typeof(object))
            {
                var cur = toCheck.IsGenericType ? toCheck.GetGenericTypeDefinition() : toCheck;
                if (generic == cur) return true;
                toCheck = toCheck.BaseType;
            }
            return false;
        }
    }
}
