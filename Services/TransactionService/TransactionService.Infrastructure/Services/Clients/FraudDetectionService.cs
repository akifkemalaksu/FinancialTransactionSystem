using Microsoft.Extensions.Logging;
using ServiceDefaults.Dtos.Responses;
using TransactionService.Application.Dtos.Clients.FraudDetection;
using TransactionService.Application.Services.Clients;

namespace TransactionService.Infrastructure.Services.Clients
{
    public class FraudDetectionService(
        HttpClient httpClient,
        ILogger<FraudDetectionService> logger
    ) : BaseHttpClient(httpClient, logger), IFraudDetectionService
    {
        public async Task<bool> IsFraudulentAsync(FraudCheckRequest request, CancellationToken cancellationToken = default)
        {
            string path = "/api/fraud-check";

            var response = await SendAsync<FraudCheckRequest, ApiResponse<FraudCheckResponse>>(HttpMethod.Post, path, request, cancellationToken: cancellationToken);

            if (response is null || response.Data is null)
            {
                return false;
            }

            return response.Data.IsFraudulent;
        }
    }
}
