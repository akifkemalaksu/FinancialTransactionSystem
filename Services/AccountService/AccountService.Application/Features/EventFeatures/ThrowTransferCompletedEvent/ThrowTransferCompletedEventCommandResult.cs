namespace AccountService.Application.Features.EventFeatures.ThrowTransferCompletedEvent
{
    public record ThrowTransferCompletedEventCommandResult
    {
        public Guid EventId { get; init; }
    }
}
