using Messaging.Abstractions;

namespace Messaging.Contracts
{
    public abstract record KafkaEvent : IEvent
    {
        public Guid Id { get; init; } = Guid.NewGuid();
    }
}
