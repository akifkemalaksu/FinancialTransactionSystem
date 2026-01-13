using AccountService.Application.Features.AccountFeatures.UpdateAccountBalance;
using AccountService.Application.Features.EventFeatures.ThrowTransferCompletedEvent;
using Messaging.Abstractions;
using Messaging.Contracts;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.EventHandlers
{
    public class TransferCreatedEventHandler(
        ICommandDispatcher _commandDispatcher
    ) : IKafkaHandler<TransferCreatedEvent>
    {
        public async Task HandleAsync(TransferCreatedEvent message)
        {
            if (message.DestinationAccountNumber is null)
            {
                if ((TransactionType)message.Type == TransactionType.Withdraw)
                {
                    await UpdateAccountBalanceAsync(message.SourceAccountNumber, -message.Amount);
                }
                else if ((TransactionType)message.Type == TransactionType.Deposit)
                {
                    await UpdateAccountBalanceAsync(message.SourceAccountNumber, message.Amount);
                }

                await PublishTransferCompletedEventAsync(message);
                return;
            }

            await UpdateAccountBalanceAsync(message.SourceAccountNumber, -message.Amount);
            await UpdateAccountBalanceAsync(message.DestinationAccountNumber, message.Amount);

            await PublishTransferCompletedEventAsync(message);
        }

        private async Task UpdateAccountBalanceAsync(string accountNumber, decimal amount)
        {
            var command = new UpdateAccountBalanceCommand
            {
                AccountNumber = accountNumber,
                Amount = amount
            };
            await _commandDispatcher.DispatchAsync<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>(command);
        }

        private async Task PublishTransferCompletedEventAsync(TransferCreatedEvent message)
        {
            var throwTransferCompletedEventCommand = new ThrowTransferCompletedEventCommand
            {
                TransactionId = message.TransactionId,
                Currency = message.Currency,
                SourceAccountNumber = message.SourceAccountNumber,
                Amount = message.Amount,
                Description = message.Description,
                DestinationAccountNumber = message.DestinationAccountNumber,
                Type = message.Type
            };
            await _commandDispatcher.DispatchAsync<ThrowTransferCompletedEventCommand, ApiResponse<ThrowTransferCompletedEventCommandResult>>(throwTransferCompletedEventCommand);
        }
    }
}
