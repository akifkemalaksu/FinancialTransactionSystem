using TransactionService.Application.Dtos.Transfers;

namespace TransactionService.Application.Features.TransferFeatures.GetTransferHistory
{
    public record GetTransferHistoryQueryResult
    {
        public required TransferHistoryDto History { get; init; }
    }
}

