using AccountService.Application.Features.AccountFeatures.UpdateAccountBalance;
using AccountService.Application.Features.EventFeatures.ThrowTransferCompletedEvent;
using Confluent.Kafka;
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
                decimal newBalance = 0;
                decimal amount = 0;
                if ((TransactionType)message.Type == TransactionType.Withdraw)
                {
                    amount = -message.Amount;
                    newBalance = await UpdateAccountBalanceAsync(message.SourceAccountNumber, amount);
                }
                else if ((TransactionType)message.Type == TransactionType.Deposit)
                {
                    amount = message.Amount;
                    newBalance = await UpdateAccountBalanceAsync(message.SourceAccountNumber, amount);
                }
                
                await PublishTransferCompletedEventAsync(message, message.SourceAccountNumber, newBalance, amount);
                return;
            }

            decimal sourceAmount = -message.Amount;
            decimal sourceNewBalance = await UpdateAccountBalanceAsync(message.SourceAccountNumber, sourceAmount);
            await PublishTransferCompletedEventAsync(message, message.SourceAccountNumber, sourceNewBalance, sourceAmount);

            decimal destinationAmount = message.Amount;
            decimal destinationNewBalance = await UpdateAccountBalanceAsync(message.DestinationAccountNumber, destinationAmount);
            await PublishTransferCompletedEventAsync(message, message.DestinationAccountNumber, destinationNewBalance, destinationAmount);
        }

        private async Task PublishTransferCompletedEventAsync(TransferCreatedEvent message, string accountNumber, decimal balance, decimal amount)
        {
            var throwTransferCompletedEventCommand = new ThrowTransferCompletedEventCommand
            {
                TransactionId = message.TransactionId,
                Currency = message.Currency,
                AccountNumber = accountNumber,
                Description = message.Description,
                Balance = balance,
                Amount = amount
            };
            await _commandDispatcher.DispatchAsync<ThrowTransferCompletedEventCommand, ApiResponse<ThrowTransferCompletedEventCommandResult>>(throwTransferCompletedEventCommand);
        }

        private async Task<decimal> UpdateAccountBalanceAsync(string accountNumber, decimal amount)
        {
            var command = new UpdateAccountBalanceCommand
            {
                AccountNumber = accountNumber,
                Amount = amount
            };
            var response = await _commandDispatcher.DispatchAsync<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>(command);

            if (!response.IsSuccess)
            {
                throw new Exception($"Failed to update account balance for account {accountNumber}: {response.Message}");
            }

            return response.Data.NewBalance;
        }
    }
}
