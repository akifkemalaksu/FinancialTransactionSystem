using LedgerService.Application.Services.DataAccessors;
using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace LedgerService.Application.Features.LedgerFeatures.GetLedgersByAccountNumber
{
    public class GetLedgersByAccountNumberQueryHandler(
        IUnitOfWork _unitOfWork
    ) : IQueryHandler<GetLedgersByAccountNumberQuery, ApiResponse<GetLedgersByAccountNumberQueryResult>>
    {
        public async Task<ApiResponse<GetLedgersByAccountNumberQueryResult>> HandleAsync(GetLedgersByAccountNumberQuery query, CancellationToken cancellationToken = default)
        {
            var ledgers = await _unitOfWork.Ledgers.GetByAccountNumberAsync(query.AccountNumber, cancellationToken);

            var result = new GetLedgersByAccountNumberQueryResult
            {
                Ledgers = ledgers
            };

            return ApiResponse<GetLedgersByAccountNumberQueryResult>.Success(
                StatusCodes.Status200OK,
                result
            );
        }
    }
}
