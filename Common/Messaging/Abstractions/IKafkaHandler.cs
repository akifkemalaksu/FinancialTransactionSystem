namespace Messaging.Abstractions
{
    public interface IKafkaHandler<TMessage> where TMessage : IEvent
    {
        Task HandleAsync(TMessage message);
    }
}
