using Microsoft.Extensions.Logging;
using ServiceDefaults.Dtos.Responses;
using TransactionService.Application.Dtos.Clients.Account;
using TransactionService.Application.Services.Clients;

namespace TransactionService.Infrastructure.Services.Clients
{
    public class AccountService(
        IHttpClientFactory httpClientFactory,
        ILogger<AccountService> logger
        ) : BaseHttpClient(httpClientFactory.CreateClient(nameof(AccountService)), logger), IAccountService
    {
        public async Task<AccountDto?> GetByAccountNumberAsync(string accountNumber, CancellationToken cancellationToken = default)
        {
            string path = $"/api/accounts/by-account-number/{accountNumber}";

            var response = await SendAsync<ApiResponse<GetByAccountNumberResponse>>(HttpMethod.Get, path, cancellationToken: cancellationToken);

            return response?.Data?.Account;
        }
    }
}