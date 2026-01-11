using AccountService.Application.Dtos.AccountDtos;
using AccountService.Application.Services.DataAccessors;
using AccountService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Services.DataAccessors
{
    public class AccountRepository(
        AccountDbContext _dbContext
    ) : IAccountRepository
    {
        public async Task<List<AccountDto>> GetAccountsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default)
        {
            var accounts = await _dbContext.Accounts
                .Where(a => a.ClientId == clientId)
                .Select(x => new AccountDto
                {
                    ClientId = x.ClientId,
                    Balance = x.Balance,
                    Currency = x.Currency,
                    CreatedAt = x.CreatedAt,
                    AccountNumber = x.AccountNumber,
                    Id = x.Id,
                    IsActive = x.IsActive
                })
                .ToListAsync(cancellationToken);

            return accounts;
        }

        public async Task<AccountDto?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default)
        {
            var account = await _dbContext.Accounts
                .Where(a => a.Id == accountId)
                .Select(x => new AccountDto
                {
                    ClientId = x.ClientId,
                    Balance = x.Balance,
                    Currency = x.Currency,
                    CreatedAt = x.CreatedAt,
                    AccountNumber = x.AccountNumber,
                    Id = x.Id,
                    IsActive = x.IsActive
                })
                .FirstOrDefaultAsync(cancellationToken);

            return account;
        }
    }
}
