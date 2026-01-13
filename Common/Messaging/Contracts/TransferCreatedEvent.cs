using Messaging.Attributes;

namespace Messaging.Contracts
{
    [KafkaTopic("transfer-created")]
    public record TransferCreatedEvent : KafkaEvent
    {
        public Guid TransactionId { get; init; }
        public required string SourceAccountNumber { get; init; }
        public string? DestinationAccountNumber { get; init; }
        public decimal Amount { get; init; }
        public required string Currency { get; init; }
        public DateTime TransactionDate { get; init; }
        public string? Description { get; init; }
        public int Type { get; init; }
    }
}
