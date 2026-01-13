using AccountService.Application.Dtos.Accounts;
using AccountService.Application.Services.DataAccessors;
using AccountService.Application.Services.InfrastructureServices;
using AccountService.Domain.Constants;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.Features.AccountFeatures.UpdateAccountBalance
{
    public class UpdateAccountBalanceCommandHandler(
        IUnitOfWork _unitOfWork,
        IDistributedCacheService _distributedCacheService
    ) : ICommandHandler<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>
    {
        public async Task<ApiResponse<UpdateAccountBalanceCommandResult>> HandleAsync(UpdateAccountBalanceCommand command, CancellationToken cancellationToken = default)
        {
            var account = await _unitOfWork.Accounts.GetEntityByAccountNumberAsync(command.AccountNumber, cancellationToken);

            if (account == null)
            {
                return ApiResponse<UpdateAccountBalanceCommandResult>.Failure(
                    StatusCodes.Status404NotFound,
                    "Account not found"
                );
            }

            account.Balance += command.Amount;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            await UpdateClientAccountsCacheAsync(account.ClientId, account.AccountNumber, account.Balance);

            return ApiResponse<UpdateAccountBalanceCommandResult>.Success(
                StatusCodes.Status200OK,
                new UpdateAccountBalanceCommandResult
                {
                    IsSuccess = true,
                    NewBalance = account.Balance
                }
            );
        }

        private async Task UpdateClientAccountsCacheAsync(Guid clientId, string accountNumber, decimal newBalance)
        {
            string cacheName = string.Format(DistributedCacheNames.GetAccountsByClientId, clientId);
            var accountsFromCache = await _distributedCacheService.GetAsync<List<AccountDto>>(cacheName);

            if (accountsFromCache is not null)
            {
                var accountFromCache = accountsFromCache.FirstOrDefault(x => x.AccountNumber == accountNumber);
                if (accountFromCache is not null)
                {
                    accountsFromCache.Remove(accountFromCache);
                    accountsFromCache.Add(accountFromCache with { Balance = newBalance });
                    await _distributedCacheService.SetAsync(cacheName, accountsFromCache);
                }
            }
        }
    }
}
