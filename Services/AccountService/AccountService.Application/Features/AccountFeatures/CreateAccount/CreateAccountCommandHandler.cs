using AccountService.Application.Dtos.Accounts;
using AccountService.Application.Services.DataAccessors;
using AccountService.Application.Services.InfrastructureServices;
using AccountService.Domain.Constants;
using AccountService.Domain.Entities;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.Features.AccountFeatures.CreateAccount
{
    public class CreateAccountCommandHandler(
        IUnitOfWork _unitOfWork,
        IDistributedCacheService _distributedCacheService
    ) : ICommandHandler<CreateAccountCommand, ApiResponse<CreateAccountCommandResult>>
    {
        public async Task<ApiResponse<CreateAccountCommandResult>> HandleAsync(CreateAccountCommand command, CancellationToken cancellationToken = default)
        {
            var client = await _unitOfWork.Clients.GetByIdAsync(command.ClientId, cancellationToken);
            if (client is null)
            {
                return ApiResponse<CreateAccountCommandResult>.Failure(
                    StatusCodes.Status404NotFound,
                    "Client not found"
                );
            }

            var accountNumber = new Random().NextInt64(1000000000, 9999999999).ToString();

            var account = new Account
            {
                ClientId = command.ClientId,
                Currency = command.Currency,
                AccountNumber = accountNumber,
                Balance = 0,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Accounts.Add(account);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await UpdateClientAccountsCacheAsync(command.ClientId, account, cancellationToken);

            return ApiResponse<CreateAccountCommandResult>.Success(
                StatusCodes.Status201Created,
                new CreateAccountCommandResult
                {
                    Id = account.Id,
                    AccountNumber = account.AccountNumber
                }
            );
        }

        private async Task UpdateClientAccountsCacheAsync(Guid clientId, Account account, CancellationToken cancellationToken)
        {
            string cacheName = string.Format(DistributedCacheNames.GetAccountsByClientId, clientId);
            var accountsFromCache = await _distributedCacheService.GetAsync<List<AccountDto>>(cacheName);

            if (accountsFromCache is null)
            {
                var accounts = await _unitOfWork.Accounts.GetAccountsByClientIdAsync(clientId, cancellationToken);
                await _distributedCacheService.SetAsync(cacheName, accounts);
            }
            else
            {
                accountsFromCache.Add(new AccountDto
                {
                    Id = account.Id,
                    AccountNumber = account.AccountNumber,
                    Currency = account.Currency,
                    Balance = account.Balance,
                    CreatedAt = account.CreatedAt,
                    ClientId = account.ClientId
                });

                await _distributedCacheService.SetAsync(cacheName, accountsFromCache);
            }
        }
    }
}
