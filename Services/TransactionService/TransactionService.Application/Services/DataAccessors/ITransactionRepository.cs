using TransactionService.Application.Dtos.TransactionDtos;
using TransactionService.Domain.Entities;

namespace TransactionService.Application.Services.DataAccessors
{
    public interface ITransactionRepository
    {
        Task<TransactionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<TransactionHistoryDto> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
        Task CreateAsync(Transaction transaction, CancellationToken cancellationToken = default);
    }
}
