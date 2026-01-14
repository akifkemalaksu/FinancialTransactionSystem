using Microsoft.Extensions.Logging;
using ServiceDefaults.Dtos.Responses;
using TransactionService.Application.Dtos.Clients.FraudDetection;
using TransactionService.Application.Services.Clients;

namespace TransactionService.Infrastructure.Services.Clients
{
    public class FraudDetectionApiService(
        IHttpClientFactory httpClientFactory,
        ILogger<FraudDetectionApiService> logger
    ) : BaseHttpClient(httpClientFactory.CreateClient(nameof(FraudDetectionApiService)), logger), IFraudDetectionApiService
    {
        public async Task<bool> IsFraudulentAsync(FraudCheckRequest request, CancellationToken cancellationToken = default)
        {
            string path = "/api/FraudCheck";

            var response = await SendAsync<FraudCheckRequest, ApiResponse<FraudCheckResponse>>(HttpMethod.Post, path, request, cancellationToken: cancellationToken);

            if (response is null || response.Data is null)
            {
                return false;
            }

            return response.Data.IsFraudulent;
        }
    }
}
