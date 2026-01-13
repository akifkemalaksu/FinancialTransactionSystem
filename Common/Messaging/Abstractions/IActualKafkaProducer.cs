namespace Messaging.Abstractions
{
    public interface IActualKafkaProducer
    {
        Task PublishToKafkaAsync(string topic, string content, CancellationToken cancellationToken);
    }
}
