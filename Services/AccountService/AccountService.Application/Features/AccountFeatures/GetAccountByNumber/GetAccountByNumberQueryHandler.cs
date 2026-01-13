using AccountService.Application.Services.DataAccessors;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.Features.AccountFeatures.GetAccountByNumber
{
    public class GetAccountByNumberQueryHandler(
        IUnitOfWork _unitOfWork
    ) : IQueryHandler<GetAccountByNumberQuery, ApiResponse<GetAccountByNumberQueryResult>>
    {
        public async Task<ApiResponse<GetAccountByNumberQueryResult>> HandleAsync(GetAccountByNumberQuery query, CancellationToken cancellationToken = default)
        {
            var account = await _unitOfWork.Accounts.GetByAccountNumberAsync(query.AccountNumber, cancellationToken);

            if (account is null)
                return ApiResponse<GetAccountByNumberQueryResult>.Failure(StatusCodes.Status404NotFound, $"Account with number {query.AccountNumber} not found");

            var result = new GetAccountByNumberQueryResult() { Account = account };

            return ApiResponse<GetAccountByNumberQueryResult>.Success(
                StatusCodes.Status200OK,
                result
            );
        }
    }
}
