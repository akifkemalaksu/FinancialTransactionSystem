using TransactionService.Application.Services.DataAccessors;
using TransactionService.Infrastructure.Data;

namespace TransactionService.Infrastructure.Services.DataAccessors
{
    public class UnitOfWork(
        TransactionDbContext _context
    ) : IUnitOfWork
    {
        private ITransferRepository? _transfers;

        public ITransferRepository Transfers => _transfers ??= new TransferRepository(_context);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);
    }
}
