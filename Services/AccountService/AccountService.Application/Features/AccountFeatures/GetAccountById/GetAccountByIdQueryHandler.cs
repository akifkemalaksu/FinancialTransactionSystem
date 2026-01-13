using AccountService.Application.Services.DataAccessors;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.Features.AccountFeatures.GetAccountById
{
    public class GetAccountByIdQueryHandler(
        IUnitOfWork _unitOfWork
    ) : IQueryHandler<GetAccountByIdQuery, ApiResponse<GetAccountByIdQueryResult>>
    {
        public async Task<ApiResponse<GetAccountByIdQueryResult>> HandleAsync(GetAccountByIdQuery query, CancellationToken cancellationToken = default)
        {
            var account = await _unitOfWork.Accounts.GetByIdAsync(query.AccountId, cancellationToken);

            if (account is null)
                return ApiResponse<GetAccountByIdQueryResult>.Failure(StatusCodes.Status404NotFound, $"Account with id {query.AccountId} not found");

            var result = new GetAccountByIdQueryResult() { Account = account };

            return ApiResponse<GetAccountByIdQueryResult>.Success(
                StatusCodes.Status200OK,
                result
            );
        }
    }
}
