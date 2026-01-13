using LedgerService.Application.Services.DataAccessors;
using LedgerService.Infrastructure.Data;

namespace LedgerService.Infrastructure.Services.DataAccessors
{
    public class UnitOfWork(
        LedgerDbContext _context
    ) : IUnitOfWork
    {
        private ILedgerRepository? _ledgers;

        public ILedgerRepository Ledgers => _ledgers ??= new LedgerRepository(_context);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);

        public void Dispose() => _context.Dispose();
    }
}
