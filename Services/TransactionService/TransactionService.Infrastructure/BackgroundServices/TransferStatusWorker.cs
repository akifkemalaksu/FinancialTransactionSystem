using Messaging.Abstractions;
using Messaging.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TransactionService.Application.Services.Clients;
using TransactionService.Application.Services.DataAccessors;
using TransactionService.Domain.Constants;

namespace TransactionService.Infrastructure.BackgroundServices
{
    public class TransferStatusWorker(
        IServiceProvider serviceProvider,
        ILogger<TransferStatusWorker> logger
    ) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = serviceProvider.CreateScope();
                    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                    var apiService = scope.ServiceProvider.GetRequiredService<ITransferApiService>();
                    var kafkaProducer = scope.ServiceProvider.GetRequiredService<IKafkaProducer>();

                    var pendingTransfers = await unitOfWork.Transfers.GetPendingAsync(50, stoppingToken);

                    if (pendingTransfers.Count != 0)
                    {
                        foreach (var transfer in pendingTransfers)
                        {
                            var result = await apiService.ProcessAsync(transfer, stoppingToken);

                            if (result.IsSuccessful)
                            {
                                transfer.Status = TransactionStatusEnum.Completed;
                            }
                            else
                            {
                                transfer.Status = TransactionStatusEnum.Failed;

                                var failedEvent = new TransferFailedEvent
                                {
                                    TransactionId = transfer.Id,
                                    SourceAccountNumber = transfer.SourceAccountNumber,
                                    DestinationAccountNumber = transfer.DestinationAccountNumber,
                                    Amount = transfer.Amount,
                                    Currency = transfer.Currency,
                                    Description = transfer.Description,
                                    Type = transfer.Type,
                                    FailureReason = result.FailureReason ?? "Unknown failure"
                                };

                                await kafkaProducer.ProduceAsync(failedEvent, stoppingToken);
                            }
                        }

                        await unitOfWork.SaveChangesAsync(stoppingToken);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "Error occurred while processing transfer statuses.");
                }

                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}
