using TransactionService.Application.Dtos.Clients.FraudDetection;

namespace TransactionService.Application.Services.Clients
{
    public interface IFraudDetectionService
    {
        Task<bool> IsFraudulentAsync(FraudCheckRequest request, CancellationToken cancellationToken = default);
    }
}