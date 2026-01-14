using Messaging.Attributes;

namespace Messaging.Contracts
{
    [KafkaTopic("transfer-completed")]
    public record TransferCompletedEvent : KafkaEvent
    {
        public Guid TransactionId { get; init; }
        public required string AccountNumber { get; init; }
        public required string Currency { get; init; }
        public string? Description { get; init; }
        public decimal Amount { get; init; }
        public decimal BalanceAfter { get; init; }
    }
}
