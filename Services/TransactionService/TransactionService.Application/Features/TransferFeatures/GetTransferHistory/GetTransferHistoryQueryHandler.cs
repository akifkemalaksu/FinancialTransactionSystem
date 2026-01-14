using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;
using TransactionService.Application.Dtos.Transfers;
using TransactionService.Application.Services.DataAccessors;

namespace TransactionService.Application.Features.TransferFeatures.GetTransferHistory
{
    public class GetTransferHistoryQueryHandler(
        IUnitOfWork _unitOfWork
    ) : IQueryHandler<GetTransferHistoryQuery, ApiResponse<GetTransferHistoryQueryResult>>
    {
        public async Task<ApiResponse<GetTransferHistoryQueryResult>> HandleAsync(GetTransferHistoryQuery query, CancellationToken cancellationToken = default)
        {
            TransferHistoryDto history = await _unitOfWork.Transfers.GetByAccountNumberAsync(query.AccountNumber, cancellationToken);

            var result = new GetTransferHistoryQueryResult
            {
                History = history
            };

            return ApiResponse<GetTransferHistoryQueryResult>.Success(
                StatusCodes.Status200OK,
                result
            );
        }
    }
}

