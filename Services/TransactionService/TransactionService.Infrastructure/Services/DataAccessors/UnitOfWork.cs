using TransactionService.Application.Services.DataAccessors;
using TransactionService.Infrastructure.Data;

namespace TransactionService.Infrastructure.Services.DataAccessors
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TransactionDbContext _context;

        public UnitOfWork(TransactionDbContext context, ITransactionRepository transactionRepository)
        {
            _context = context;
            Transactions = transactionRepository;
        }

        public ITransactionRepository Transactions { get; }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
