using TransactionService.Domain.Entities;
using TransactionService.Application.Dtos.Transfers;

namespace TransactionService.Application.Services.DataAccessors
{
    public interface ITransferRepository
    {
        Task<TransferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TransferDto?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);
        Task<TransferHistoryDto> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
        Task CreateAsync(Transfer transfer, CancellationToken cancellationToken = default);
    }
}
