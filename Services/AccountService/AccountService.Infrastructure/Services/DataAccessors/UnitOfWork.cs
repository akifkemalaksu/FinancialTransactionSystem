using AccountService.Application.Services.DataAccessors;
using AccountService.Infrastructure.Data;

namespace AccountService.Infrastructure.Services.DataAccessors
{
    public class UnitOfWork(
        AccountDbContext _context
    ) : IUnitOfWork
    {
        private IAccountRepository? _accounts;
        private IClientRepository? _clients;

        public IAccountRepository Accounts => _accounts ??= new AccountRepository(_context);
        public IClientRepository Clients => _clients ??= new ClientRepository(_context);

        public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) => _context.SaveChangesAsync(cancellationToken);

        public void Dispose() => _context.Dispose();
        
    }
}
