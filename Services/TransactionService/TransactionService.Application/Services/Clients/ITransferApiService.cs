using TransactionService.Application.Dtos.Clients.Transfer;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Services.Clients
{
    public interface ITransferApiService
    {
        Task<TransferProcessingResult> ProcessAsync(Transfer transfer, CancellationToken cancellationToken = default);
    }
}
