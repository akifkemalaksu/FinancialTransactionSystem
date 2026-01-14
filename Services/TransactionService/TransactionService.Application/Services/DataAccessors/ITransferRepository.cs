using TransactionService.Application.Dtos.Transfers;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Services.DataAccessors
{
    public interface ITransferRepository
    {
        Task<TransferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TransferDto?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default);
        Task<TransferHistoryDto> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
        Task<List<Transfer>> GetPendingAsync(int take, CancellationToken cancellationToken = default);
        void Add(Transfer transfer);
    }
}
