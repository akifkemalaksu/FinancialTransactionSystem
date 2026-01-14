using LedgerService.Application.Services.DataAccessors;
using LedgerService.Domain.Entities;
using LedgerService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LedgerService.Infrastructure.Services.DataAccessors
{
    public class LedgerRepository(
        LedgerDbContext _context
    ) : ILedgerRepository
    {
        public void Add(Ledger ledger)
        {
            _context.Ledgers.Add(ledger);
        }

        public Task<List<Ledger>> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            return _context.Ledgers
                .Where(l => l.AccountNumber == accountNumber)
                .OrderByDescending(l => l.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}
