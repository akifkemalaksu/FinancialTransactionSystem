using TransactionService.Application.Dtos.Clients.Transfer;
using TransactionService.Application.Services.Clients;
using TransactionService.Domain.Entities;

namespace TransactionService.Infrastructure.Services.Clients
{
    public class MockTransferApiService : ITransferApiService
    {
        public Task<TransferProcessingResult> ProcessAsync(Transfer transfer, CancellationToken cancellationToken = default)
        {
            var isSuccessful = transfer.Amount <= 10000m;

            var result = new TransferProcessingResult
            {
                IsSuccessful = isSuccessful,
                FailureReason = isSuccessful ? null : "Mock transfer processing failed"
            };

            return Task.FromResult(result);
        }
    }
}
