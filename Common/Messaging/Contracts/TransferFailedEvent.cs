using Messaging.Attributes;

namespace Messaging.Contracts
{
    [KafkaTopic("transfer-failed")]
    public record TransferFailedEvent : KafkaEvent
    {
        public Guid TransactionId { get; init; }
        public required string SourceAccountNumber { get; init; }
        public string? DestinationAccountNumber { get; init; }
        public decimal Amount { get; init; }
        public required string Currency { get; init; }
        public string? Description { get; init; }
        public int Type { get; init; }
        public required string FailureReason { get; init; }
    }
}

