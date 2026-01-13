using Messaging.Abstractions;

namespace Messaging.Contracts
{
    public record TransferCreatedEvent : IKeyedMessage
    {
        public Guid TransactionId { get; init; }
        public required string SourceAccountNumber { get; init; }
        public string? DestinationAccountNumber { get; init; }
        public decimal Amount { get; init; }
        public required string Currency { get; init; }
        public DateTime TransactionDate { get; init; }
        public string? Description { get; init; }

        public string Key => TransactionId.ToString();
    }
}
