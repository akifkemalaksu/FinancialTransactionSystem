using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Dtos.TransactionDtos;
using TransactionService.Application.Services.DataAccessors;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Data;

namespace TransactionService.Infrastructure.Services.DataAccessors
{
    public class TransactionRepository(TransactionDbContext _dbContext) : ITransactionRepository
    {
        public async Task CreateAsync(Transaction transaction, CancellationToken cancellationToken = default)
        {
            await _dbContext.Transactions.AddAsync(transaction, cancellationToken);
        }

        public async Task<TransactionHistoryDto> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            var transactions = await _dbContext.Transactions
                .Where(t => t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    SourceAccountNumber = t.SourceAccountNumber,
                    DestinationAccountNumber = t.DestinationAccountNumber,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status,
                    Description = t.Description
                })
                .ToListAsync(cancellationToken);

            return new TransactionHistoryDto
            {
                IncomingTransactions = transactions.Where(t => t.DestinationAccountNumber == accountNumber).ToList(),
                OutgoingTransactions = transactions.Where(t => t.SourceAccountNumber == accountNumber).ToList()
            };
        }

        public async Task<TransactionDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transactions
                .Where(t => t.Id == id)
                .Select(t => new TransactionDto
                {
                    Id = t.Id,
                    SourceAccountNumber = t.SourceAccountNumber,
                    DestinationAccountNumber = t.DestinationAccountNumber,
                    Amount = t.Amount,
                    Currency = t.Currency,
                    TransactionDate = t.TransactionDate,
                    Status = t.Status,
                    Description = t.Description
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
