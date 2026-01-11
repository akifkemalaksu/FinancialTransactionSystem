using MassTransit;
using Messaging.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Messaging.Internal
{
    public class MessageBus(
        IPublishEndpoint _publishEndpoint,
        IServiceProvider _serviceProvider
    ) : IMessageBus
    {
        public async Task ProduceAsync<T>(string key, T message, CancellationToken cancellationToken = default) where T : class
        {
            var producer = _serviceProvider.GetRequiredService<ITopicProducer<string, T>>();

            await producer.Produce(key, message, cancellationToken);
        }

        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            return _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
