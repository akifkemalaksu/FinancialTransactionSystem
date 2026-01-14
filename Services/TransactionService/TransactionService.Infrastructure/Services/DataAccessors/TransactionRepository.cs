using Microsoft.EntityFrameworkCore;
using ServiceDefaults.Enums;
using TransactionService.Application.Dtos.Transfers;
using TransactionService.Application.Services.DataAccessors;
using TransactionService.Domain.Constants;
using TransactionService.Domain.Entities;
using TransactionService.Infrastructure.Data;

namespace TransactionService.Infrastructure.Services.DataAccessors
{
    public class TransferRepository(TransactionDbContext _dbContext) : ITransferRepository
    {
        public void Add(Transfer transfer) => _dbContext.Transfers.Add(transfer);

        public async Task<List<Transfer>> GetPendingAsync(int take, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Transfers
                .Where(t => t.Status == TransactionStatusEnum.Pending)
                .OrderBy(t => t.TransactionDate)
                .Take(take)
                .ToListAsync(cancellationToken);
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
                    Type = (TransactionType)t.Type,
                    Description = t.Description
                })
                .ToListAsync(cancellationToken);

            return new TransferHistoryDto
            {
                IncomingTransfers = transfers.Where(t => t.DestinationAccountNumber == accountNumber || (t.SourceAccountNumber == accountNumber && t.Type == TransactionType.Deposit)).ToList(),
                OutgoingTransfers = transfers.Where(t => t.SourceAccountNumber == accountNumber && t.Type != TransactionType.Deposit).ToList()
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
                    Type = (TransactionType)t.Type,
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
                    Type = (TransactionType)t.Type,
                    Description = t.Description
                })
                .FirstOrDefaultAsync(cancellationToken);
        }
    }
}
