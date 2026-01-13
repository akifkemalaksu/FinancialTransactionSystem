using Microsoft.AspNetCore.Http;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace FraudDetectionService.Application.Features.FraudCheck
{
    public class CheckFraudQueryHandler : IQueryHandler<CheckFraudQuery, ApiResponse<CheckFraudQueryResult>>
    {
        public Task<ApiResponse<CheckFraudQueryResult>> HandleAsync(CheckFraudQuery query, CancellationToken cancellationToken = default)
        {
            if (query.Amount > 1_000_000)
            {
                return Task.FromResult(ApiResponse<CheckFraudQueryResult>.Success(
                    StatusCodes.Status200OK,
                    new CheckFraudQueryResult
                    {
                        IsFraudulent = true,
                        Reason = "Amount exceeds the maximum limit for a single transaction."
                    }
                ));
            }

            return Task.FromResult(ApiResponse<CheckFraudQueryResult>.Success(
                StatusCodes.Status200OK,
                new CheckFraudQueryResult
                {
                    IsFraudulent = false,
                    Reason = null
                }
            ));
        }
    }
}
