namespace TransactionService.Application.Features.TransferFeatures.GetTransferHistory
{
    public record GetTransferHistoryQuery
    {
        public required string AccountNumber { get; init; }
    }
}

