namespace Messaging.Abstractions
{
    public interface IActualKafkaProducer
    {
        Task PublishToKafkaAsync(string topic, string key, string content, CancellationToken cancellationToken);
    }
}
