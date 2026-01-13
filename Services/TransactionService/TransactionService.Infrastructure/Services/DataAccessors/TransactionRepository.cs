using Microsoft.EntityFrameworkCore;
using TransactionService.Application.Dtos.Transfers;
using TransactionService.Application.Services.DataAccessors;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Data;

namespace TransactionService.Infrastructure.Services.DataAccessors
{
    public class TransferRepository(TransactionDbContext _dbContext) : ITransferRepository
    {
        public async Task CreateAsync(Transfer transfer, CancellationToken cancellationToken = default)
        {
            await _dbContext.Transfers.AddAsync(transfer, cancellationToken);
        }

        public async Task<TransferHistoryDto> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            var transfers = await _dbContext.Transfers
                .Where(t => t.SourceAccountNumber == accountNumber || t.DestinationAccountNumber == accountNumber)
                .Select(t => new TransferDto
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

            return new TransferHistoryDto
            {
                IncomingTransfers = transfers.Where(t => t.DestinationAccountNumber == accountNumber).ToList(),
                OutgoingTransfers = transfers.Where(t => t.SourceAccountNumber == accountNumber).ToList()
            };
        }

        public async Task<TransferDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transfers
                .Where(t => t.Id == id)
                .Select(t => new TransferDto
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

        public async Task<TransferDto?> GetByIdempotencyKeyAsync(string idempotencyKey, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transfers
                .Where(t => t.IdempotencyKey == idempotencyKey)
                .Select(t => new TransferDto
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
