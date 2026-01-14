using Messaging.Abstractions;
using Messaging.Contracts;
using NotificationService.Application.Features.NotificationFeatures.CreateNotification;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace NotificationService.Application.EventHandlers
{
    public class TransferFailedEventHandler(
        ICommandDispatcher commandDispatcher
    ) : IKafkaHandler<TransferFailedEvent>
    {
        public async Task HandleAsync(TransferFailedEvent message)
        {
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

