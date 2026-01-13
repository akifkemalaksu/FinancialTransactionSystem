using Messaging.Abstractions;
using Messaging.Contracts;
using NotificationService.Application.Features.NotificationFeatures.CreateNotification;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace NotificationService.Application.EventHandlers
{
    public class TransferCreatedHandler(
        ICommandDispatcher _commandDispatcher
    ) : IKafkaHandler<TransferCreatedEvent>
    {
        public async Task HandleAsync(TransferCreatedEvent message)
        {
            CreateNotificationCommand command;

            if (string.IsNullOrEmpty(message.DestinationAccountNumber))
            {
                command = new CreateNotificationCommand
                {
                    Title = "Balance Update",
                    Message = $"{Math.Abs(message.Amount)} {message.Currency} transaction processed for account {message.SourceAccountNumber}. {message.Description}"
                };
            }
            else
            {
                command = new CreateNotificationCommand
                {
                    Title = "Transfer Successful",
                    Message = $"From {message.SourceAccountNumber} to {message.DestinationAccountNumber} {message.Amount} {message.Currency} sent."
                };
            }

            await _commandDispatcher.DispatchAsync<CreateNotificationCommand, ApiResponse<CreateNotificationCommandResult>>(command);
        }
    }
}
