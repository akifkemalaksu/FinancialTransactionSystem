
using MassTransit;
using Messaging.Abstractions;
using Messaging.Configurations;
using Messaging.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;


namespace Messaging.Extensions
{
    public static class MessagingExtensions
    {
        public static IServiceCollection AddMessagingBus(this IServiceCollection services, IConfiguration configuration)
        {
            var settings = configuration.GetRequiredSection(nameof(KafkaSettings)).Get<KafkaSettings>()!;

            services.AddMassTransit(x =>
            {
                x.UsingInMemory((context, cfg) => cfg.ConfigureEndpoints(context));

                x.AddRider(rider =>
                {
                    rider.AddConsumers(Assembly.GetCallingAssembly());

                    rider.UsingKafka((context, k) =>
                    {
                        k.Host(settings.Host);

                        k.Acks = Confluent.Kafka.Acks.Leader;

                        var consumerTypes = Assembly.GetEntryAssembly()?
                            .GetTypes()
                            .Where(t => typeof(IConsumer).IsAssignableFrom(t) && !t.IsAbstract) ?? Enumerable.Empty<Type>();

                        foreach(var consumerType in consumerTypes)
                        {
                            var topicName = $"{consumerType.Name.Replace("Consumer", "").ToLower()}-topic";

                            k.TopicEndpoint<string, dynamic>(topicName, settings.GroupId, e =>
                            {
                                e.ConfigureConsumer(context, consumerType);
                            });
                        }
                    });

                    var contractTypes = Assembly.GetExecutingAssembly()
                        .GetTypes()
                        .Where(t => t.IsInterface && t.Namespace != null && t.Namespace.Contains("Contracts"));

                    foreach (var type in contractTypes)
                    {
                        var topicName = $"{type.Name.Substring(1).ToLower()}-topic";

                        var method = typeof(KafkaProducerRegistrationExtensions)
                            .GetMethods()
                            .FirstOrDefault(m => m.Name == "AddProducer" && m.GetGenericArguments().Length == 2);

                        if (method != null)
                        {
                            var genericMethod = method.MakeGenericMethod(typeof(string), type);
                            genericMethod.Invoke(null, new object[] { rider, topicName, null, null });
                        }
                    }
                });
            });

            services.AddScoped<IMessageBus, MessageBus>();

            return services;
        }
    }
}
