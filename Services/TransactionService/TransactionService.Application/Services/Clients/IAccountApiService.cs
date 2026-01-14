using TransactionService.Application.Dtos.Clients.Account;

namespace TransactionService.Application.Services.Clients
{
    public interface IAccountApiService
    {
        Task<AccountDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default);
    }
}