using MassTransit;
using Messaging.Contracts;
using NotificationService.Application.Features.NotificationFeatures.CreateNotification;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace NotificationService.API.Consumers
{
    public class TransferCreatedConsumer(
        ICommandDispatcher _commandDispatcher
    ) : IConsumer<TransferCreatedEvent>
    {
        public async Task Consume(ConsumeContext<TransferCreatedEvent> context)
        {
            CreateNotificationCommand command;

            if (string.IsNullOrEmpty(context.Message.DestinationAccountNumber))
            {
                command = new CreateNotificationCommand
                {
                    Title = "Balance Update",
                    Message = $"{Math.Abs(context.Message.Amount)} {context.Message.Currency} transaction processed for account {context.Message.SourceAccountNumber}. {context.Message.Description}"
                };
            }
            else
            {
                command = new CreateNotificationCommand
                {
                    Title = "Transfer Successful",
                    Message = $"From {context.Message.SourceAccountNumber} to {context.Message.DestinationAccountNumber} {context.Message.Amount} {context.Message.Currency} sent."
                };
            }

            await _commandDispatcher.DispatchAsync<CreateNotificationCommand, ApiResponse<CreateNotificationCommandResult>>(command, context.CancellationToken);
        }
    }
}