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
            var command = new CreateNotificationCommand
            {
                Title = "Transfer Successful",
                Message = $"From {context.Message.SourceAccountNumber} to {context.Message.DestinationAccountNumber} {context.Message.Amount} {context.Message.Currency} sent."
            };

            await _commandDispatcher.DispatchAsync<CreateNotificationCommand, ApiResponse<CreateNotificationCommandResult>>(command, context.CancellationToken);
        }
    }
}