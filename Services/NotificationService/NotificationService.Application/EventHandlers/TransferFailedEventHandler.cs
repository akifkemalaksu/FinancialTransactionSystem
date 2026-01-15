using Messaging.Abstractions;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Features.NotificationFeatures.CreateNotification;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace NotificationService.Application.EventHandlers
{
    public class TransferFailedEventHandler(
        ICommandDispatcher commandDispatcher,
        ILogger<TransferFailedEventHandler> _logger
    ) : IKafkaHandler<TransferFailedEvent>
    {
        public async Task HandleAsync(TransferFailedEvent message)
        {
            _logger.LogWarning(
                "Handling TransferFailedEvent notification for transaction {TransactionId}, source {SourceAccount}, destination {DestinationAccount}, failure reason {FailureReason}",
                message.TransactionId,
                message.SourceAccountNumber,
                message.DestinationAccountNumber,
                message.FailureReason
            );

            var title = "Transfer Failed";
            var body = $"Transaction {message.TransactionId} failed. Reason: {message.FailureReason}";

            var command = new CreateNotificationCommand
            {
                Title = title,
                Message = body
            };

            await commandDispatcher.DispatchAsync<CreateNotificationCommand, ApiResponse<CreateNotificationCommandResult>>(command);
        }
    }
}

