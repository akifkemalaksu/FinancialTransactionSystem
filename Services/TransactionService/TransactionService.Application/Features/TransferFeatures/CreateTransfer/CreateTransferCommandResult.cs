namespace TransactionService.Application.Features.TransferFeatures.CreateTransfer
{
    public record CreateTransferCommandResult
    {
        public Guid TransactionId { get; init; }
        public string Status { get; init; } = string.Empty;
    }
}
