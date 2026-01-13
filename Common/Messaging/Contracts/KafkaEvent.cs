using Messaging.Abstractions;

namespace Messaging.Contracts
{
    public abstract record KafkaEvent : IEvent
    {
        public Guid Key { get; init; } = Guid.NewGuid();
    }
}
