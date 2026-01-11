using AccountService.Application.Dtos.AccountDtos;

namespace AccountService.Application.Services.DataAccessors
{
    public interface IAccountRepository
    {
        Task<List<AccountDto>> GetAccountsByClientIdAsync(Guid clientId, CancellationToken cancellationToken = default);
        Task<AccountDto?> GetByIdAsync(Guid accountId, CancellationToken cancellationToken = default);
    }
}
