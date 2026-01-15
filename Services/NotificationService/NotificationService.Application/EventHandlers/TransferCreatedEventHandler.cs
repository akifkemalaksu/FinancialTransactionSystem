using Messaging.Abstractions;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Features.NotificationFeatures.CreateNotification;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;

namespace NotificationService.Application.EventHandlers
{
    public class TransferCreatedEventHandler(
        ICommandDispatcher _commandDispatcher,
        ILogger<TransferCreatedEventHandler> _logger
    ) : IKafkaHandler<TransferCreatedEvent>
    {
        public async Task HandleAsync(TransferCreatedEvent message)
        {
            string title;
            string body;

            if (string.IsNullOrEmpty(message.DestinationAccountNumber))
            {
                var type = (TransactionType)message.Type;
                title = type == TransactionType.Deposit ? "Deposit Successful" : "Withdrawal Successful";
                body = $"{Math.Abs(message.Amount)} {message.Currency} {type.ToString().ToLower()} processed for account {message.SourceAccountNumber}. {message.Description}";
            }
            else
            {
                title = "Transfer Successful";
                body = $"From {message.SourceAccountNumber} to {message.DestinationAccountNumber} {message.Amount} {message.Currency} sent.";
            }

            _logger.LogInformation(
                "Handling TransferCreatedEvent notification for transaction {TransactionId}, title '{Title}'",
                message.TransactionId,
                title
            );

            var command = new CreateNotificationCommand
            {
                Title = title,
                Message = body
            };

            await _commandDispatcher.DispatchAsync<CreateNotificationCommand, ApiResponse<CreateNotificationCommandResult>>(command);
        }
    }
}
