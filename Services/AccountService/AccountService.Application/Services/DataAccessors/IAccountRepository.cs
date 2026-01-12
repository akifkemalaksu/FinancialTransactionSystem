using AccountService.Application.Dtos.Accounts;

namespace AccountService.Application.Services.DataAccessors
{
    public interface IAccountRepository
    {
        Task<List<AccountDto>> GetAccountsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
        Task<AccountDto?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
        Task<AccountDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    }
}
