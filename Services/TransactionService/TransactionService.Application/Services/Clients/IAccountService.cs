using TransactionService.Application.Dtos.Clients.Account;

namespace TransactionService.Application.Services.Clients
{
    public interface IAccountService
    {
        Task<AccountDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    }
}