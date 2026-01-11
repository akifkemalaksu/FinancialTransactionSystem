namespace Messaging.Abstractions
{
    public interface IMessageBus
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class;

        Task ProduceAsync<T>(string key, T message, CancellationToken cancellationToken = default) where T : class;
    }
}
