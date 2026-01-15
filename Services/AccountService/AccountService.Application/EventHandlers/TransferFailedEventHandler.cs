using AccountService.Application.Features.AccountFeatures.UpdateAccountBalance;
using Messaging.Abstractions;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;

namespace AccountService.Application.EventHandlers
{
    public class TransferFailedEventHandler(
        ICommandDispatcher commandDispatcher,
        ILogger<TransferFailedEventHandler> _logger
    ) : IKafkaHandler<TransferFailedEvent>
    {
        public async Task HandleAsync(TransferFailedEvent message)
        {
            var type = (TransactionType)message.Type;

            _logger.LogWarning(
                "Handling TransferFailedEvent for transaction {TransactionId}, source {SourceAccount}, destination {DestinationAccount}, type {Type}, failure reason {FailureReason}",
                message.TransactionId,
                message.SourceAccountNumber,
                message.DestinationAccountNumber,
                type,
                message.FailureReason
            );

            if (string.IsNullOrEmpty(message.DestinationAccountNumber))
            {
                await CompensateSingleAccountAsync(message, type);
                return;
            }

            await CompensateTransferAsync(message);
        }

        private async Task CompensateSingleAccountAsync(TransferFailedEvent message, TransactionType type)
        {
            decimal amount;

            if (type == TransactionType.Withdraw)
            {
                amount = message.Amount;
            }
            else
            {
                amount = -message.Amount;
            }

            _logger.LogInformation(
                "Compensating single account {AccountNumber} with amount {Amount} for failed transaction {TransactionId}",
                message.SourceAccountNumber,
                amount,
                message.TransactionId
            );

            await UpdateAccountBalanceAsync(message.SourceAccountNumber, amount);
        }

        private async Task CompensateTransferAsync(TransferFailedEvent message)
        {
            var sourceAmount = message.Amount;
            var destinationAmount = -message.Amount;

            _logger.LogInformation(
                "Compensating transfer for failed transaction {TransactionId}. Source {SourceAccount} amount {SourceAmount}, destination {DestinationAccount} amount {DestinationAmount}",
                message.TransactionId,
                message.SourceAccountNumber,
                sourceAmount,
                message.DestinationAccountNumber,
                destinationAmount
            );

            await UpdateAccountBalanceAsync(message.SourceAccountNumber, sourceAmount);
            await UpdateAccountBalanceAsync(message.DestinationAccountNumber!, destinationAmount);
        }

        private async Task UpdateAccountBalanceAsync(string accountNumber, decimal amount)
        {
            var command = new UpdateAccountBalanceCommand
            {
                AccountNumber = accountNumber,
                Amount = amount
            };
            var response = await commandDispatcher.DispatchAsync<UpdateAccountBalanceCommand, ApiResponse<UpdateAccountBalanceCommandResult>>(command);

            if (!response.IsSuccess)
            {
                _logger.LogError(
                    "Failed to compensate account balance for account {AccountNumber}: {Message}",
                    accountNumber,
                    response.Message
                );

                throw new Exception($"Failed to compensate account balance for account {accountNumber}: {response.Message}");
            }
        }
    }
}

