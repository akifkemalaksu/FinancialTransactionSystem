
using MassTransit;
using Messaging.Abstractions;
using Messaging.Configurations;
using Messaging.Internal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Messaging.Extensions
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessagingBus<TDbContext>(
            this IServiceCollection services,
            IConfiguration configuration,
            Action<IEntityFrameworkOutboxConfigurator>? configureOutbox = null
        )
            where TDbContext : DbContext
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

            var kafkaSettings = configuration.GetRequiredSection(nameof(KafkaSettings)).Get<KafkaSettings>()!;

            var consumerTypes = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract &&
                       t.GetInterfaces().Any(i => i.IsGenericType &&
                       i.GetGenericTypeDefinition() == typeof(IConsumer<>)))
                .ToList();

            services.AddMassTransit(x =>
            {
                x.AddEntityFrameworkOutbox<TDbContext>(o =>
                {
                    configureOutbox?.Invoke(o);
                    o.UseBusOutbox();
                });

                foreach (var type in consumerTypes)
                {
                    x.AddConsumer(type);
                }

                x.AddRider(rider =>
                {
                    foreach (var type in consumerTypes)
                    {
                        rider.AddConsumer(type);
                    }

                    rider.UsingKafka((context, k) =>
                    {
                        k.Host(kafkaSettings.Host);

                        var configureKafkaTopicEndpointMethod = typeof(MessagingExtensions)
                            .GetMethod(nameof(ConfigureKafkaTopicEndpoint), BindingFlags.NonPublic | BindingFlags.Static);

                        var consumerTypes = assembly.GetTypes()
                        .Where(t => 
                            t.IsClass && 
                            !t.IsAbstract &&
                            t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))
                        );

                        string groupId = kafkaSettings.GroupId;
                        foreach (var type in consumerTypes)
                        {
                            var messageType = type.GetInterfaces()
                                .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IConsumer<>))
                                .GetGenericArguments()[0];

                            var entityNameAttribute = messageType.GetCustomAttribute<EntityNameAttribute>();
                            var topicName = entityNameAttribute?.EntityName ?? messageType.Name.ToLower();

                            if (configureKafkaTopicEndpointMethod is null)
                                throw new InvalidOperationException($"{nameof(ConfigureKafkaTopicEndpoint)} not found.");

                            var genericConfigureMethod = configureKafkaTopicEndpointMethod.MakeGenericMethod(typeof(TDbContext), messageType);

                            genericConfigureMethod.Invoke(
                                null,
                                [context, k, topicName, groupId, type]
                            );
                        }
                    });
                });

                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IMessageBus, MessageBus>();

            return services;
        }

        private static void ConfigureKafkaTopicEndpoint<TDbContext, TMessage>(
            IRiderRegistrationContext context,
            IKafkaFactoryConfigurator kafka,
            string topicName,
            string groupId,
            Type consumerType
        )
            where TDbContext : DbContext
            where TMessage : class
        {
            kafka.TopicEndpoint<string, TMessage>(topicName, groupId, e =>
            {
                e.UseEntityFrameworkOutbox<TDbContext>(context);
                e.ConfigureConsumer(context, consumerType);
            });
        }
    }
}
