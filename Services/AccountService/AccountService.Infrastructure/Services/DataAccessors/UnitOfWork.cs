using AccountService.Application.Services.DataAccessors;
using AccountService.Infrastructure.Data;

namespace AccountService.Infrastructure.Services.DataAccessors
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AccountDbContext _dbContext;
        public IAccountRepository Accounts { get; private set; }

        public UnitOfWork(AccountDbContext dbContext)
        {
            _dbContext = dbContext;
            Accounts = new AccountRepository(_dbContext);
        }

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _dbContext.SaveChangesAsync(cancellationToken);

        public void Dispose() => _dbContext.Dispose();
        
    }
}
