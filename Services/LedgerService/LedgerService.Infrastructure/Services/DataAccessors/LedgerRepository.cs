using LedgerService.Application.Services.DataAccessors;
using LedgerService.Domain.Entities;
using LedgerService.Infrastructure.Data;

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
    }
}
