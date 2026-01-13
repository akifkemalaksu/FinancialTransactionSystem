using AccountService.Application.Dtos.Accounts;
using AccountService.Domain.Entities;

namespace AccountService.Application.Services.DataAccessors
{
    public interface IAccountRepository
    {
        void Add(Account account);
        Task<List<AccountDto>> GetAccountsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
        Task<AccountDto?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task<AccountDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
        Task<Account?> GetEntityByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    }
}
