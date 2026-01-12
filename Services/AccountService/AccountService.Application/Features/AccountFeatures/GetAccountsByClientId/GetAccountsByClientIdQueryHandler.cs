using AccountService.Application.Services.DataAccessors;
using AccountService.Application.Services.InfrastructureServices;
using AccountService.Domain.Constants;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;
using AccountService.Application.Dtos.Accounts;

namespace AccountService.Application.Features.AccountFeatures.GetAccountsByClientId
{
    public class GetAccountsByClientIdQueryHandler(
        IUnitOfWork _unitOfWork,
        IDistributedCacheService _distributedCacheService
    ) : IQueryHandler<GetAccountsByClientIdQuery, ApiResponse<GetAccountsByClientIdQueryResult>>
    {
        public async Task<ApiResponse<GetAccountsByClientIdQueryResult>> HandleAsync(GetAccountsByClientIdQuery query, CancellationToken cancellationToken = default)
        {
            var cacheName = string.Format(RedisCacheNames.GetAccountsByClientId, query.ClientId);

            var exists = await _distributedCacheService.ExistsAsync(cacheName);
            if (exists)
            {
                var cachedAccounts = await _distributedCacheService.GetAsync<List<AccountDto>>(cacheName);
                if (cachedAccounts is not null)
                {
                    return ApiResponse<GetAccountsByClientIdQueryResult>.Success(StatusCodes.Status200OK, new GetAccountsByClientIdQueryResult()
                    {
                        Accounts = cachedAccounts
                    });
                }
            }

            var accounts = await _unitOfWork.Accounts.GetAccountsByClientIdAsync(query.ClientId, cancellationToken);

            if (accounts is null || accounts.Count == 0)
                return ApiResponse<GetAccountsByClientIdQueryResult>.Failure(StatusCodes.Status404NotFound, $"Accounts with client id {query.ClientId} not found");

            await _distributedCacheService.SetAsync(cacheName, accounts, TimeSpan.FromMinutes(5));

            return ApiResponse<GetAccountsByClientIdQueryResult>.Success(StatusCodes.Status200OK, new GetAccountsByClientIdQueryResult()
            {
                Accounts = accounts
            });
        }
    }
}
