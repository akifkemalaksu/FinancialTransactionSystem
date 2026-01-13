namespace Messaging.Abstractions
{
    public interface IKafkaProducer
    {
        Task ProduceAsync<T>(T message, CancellationToken cancellationToken) where T : IEvent;
    }
}
