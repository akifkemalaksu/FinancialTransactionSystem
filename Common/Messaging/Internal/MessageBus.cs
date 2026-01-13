using MassTransit;
using Messaging.Abstractions;

namespace Messaging.Internal
{
    public class MessageBus(
        IPublishEndpoint _publishEndpoint
    ) : IMessageBus
    {
        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
        {
            return _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}
