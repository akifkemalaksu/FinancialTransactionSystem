using AccountService.Application.Dtos.Accounts;
using AccountService.Application.Services.DataAccessors;
using AccountService.Domain.Entities;
using AccountService.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Services.DataAccessors
{
    public class AccountRepository(
        AccountDbContext _dbContext
    ) : IAccountRepository
    {
        public async Task AddAsync(Account account, CancellationToken cancellationToken = default)
        {
            await _dbContext.Accounts.AddAsync(account, cancellationToken);
        }

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
                    Id = x.Id
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
                    Id = x.Id
                })
                .FirstOrDefaultAsync(cancellationToken);

            return account;
        }

        public async Task<AccountDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            var account = await _dbContext.Accounts
                .Where(a => a.AccountNumber == accountNumber)
                .Select(x => new AccountDto
                {
                    ClientId = x.ClientId,
                    Balance = x.Balance,
                    Currency = x.Currency,
                    CreatedAt = x.CreatedAt,
                    AccountNumber = x.AccountNumber,
                    Id = x.Id
                })
                .FirstOrDefaultAsync(cancellationToken);

            return account;
        }

        public async Task<Account?> GetEntityByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            return await _dbContext.Accounts
                .FirstOrDefaultAsync(a => a.AccountNumber == accountNumber, cancellationToken);
        }
    }
}
