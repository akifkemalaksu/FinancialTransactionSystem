namespace AccountService.Application.Features.EventFeatures.ThrowTransferCompletedEvent
{
    public record ThrowTransferCompletedEventCommand
    {
        public Guid TransactionId { get; init; }
        public required string SourceAccountNumber { get; init; }
        public string? DestinationAccountNumber { get; init; }
        public required string Currency { get; init; }
        public string? Description { get; init; }
        public decimal Amount { get; init; }
        public int Type { get; init; }
    }
}
