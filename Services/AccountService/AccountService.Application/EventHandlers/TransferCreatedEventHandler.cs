using AccountService.Application.Features.AccountFeatures.UpdateAccountBalance;
using AccountService.Application.Features.EventFeatures.ThrowTransferCompletedEvent;
using Messaging.Abstractions;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.EventHandlers
{
    public class TransferCreatedEventHandler(
        ICommandDispatcher _commandDispatcher,
        ILogger<TransferCreatedEventHandler> _logger
    ) : IKafkaHandler<TransferCreatedEvent>
    {
        public async Task HandleAsync(TransferCreatedEvent message)
        {
            _logger.LogInformation(
                "Handling TransferCreatedEvent for transaction {TransactionId}, source {SourceAccount}, destination {DestinationAccount}, amount {Amount}",
                message.TransactionId,
                message.SourceAccountNumber,
                message.DestinationAccountNumber,
                message.Amount
            );

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

                _logger.LogInformation(
                    "Single-account transfer processed for account {AccountNumber}, new balance {NewBalance}, amount {Amount}",
                    message.SourceAccountNumber,
                    newBalance,
                    amount
                );

                await PublishTransferCompletedEventAsync(message, message.SourceAccountNumber, newBalance, amount);
                return;
            }

            decimal sourceAmount = -message.Amount;
            decimal sourceNewBalance = await UpdateAccountBalanceAsync(message.SourceAccountNumber, sourceAmount);

            _logger.LogInformation(
                "Source account {AccountNumber} updated with amount {Amount}, new balance {NewBalance}",
                message.SourceAccountNumber,
                sourceAmount,
                sourceNewBalance
            );
            
            await PublishTransferCompletedEventAsync(message, message.SourceAccountNumber, sourceNewBalance, sourceAmount);

            decimal destinationAmount = message.Amount;
            decimal destinationNewBalance = await UpdateAccountBalanceAsync(message.DestinationAccountNumber, destinationAmount);
            
            _logger.LogInformation(
                "Destination account {AccountNumber} updated with amount {Amount}, new balance {NewBalance}",
                message.DestinationAccountNumber,
                destinationAmount,
                destinationNewBalance
            );

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

            _logger.LogInformation(
                "Publishing TransferCompletedEvent for transaction {TransactionId}, account {AccountNumber}, balance {Balance}, amount {Amount}",
                message.TransactionId,
                accountNumber,
                balance,
                amount
            );

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
                _logger.LogError(
                    "Failed to update account balance for account {AccountNumber}: {Message}",
                    accountNumber,
                    response.Message
                );

                throw new Exception($"Failed to update account balance for account {accountNumber}: {response.Message}");
            }

            return response.Data.NewBalance;
        }
    }
}
