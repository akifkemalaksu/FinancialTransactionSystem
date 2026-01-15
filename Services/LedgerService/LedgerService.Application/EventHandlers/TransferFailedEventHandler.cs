using LedgerService.Application.Features.LedgerFeatures.CreateLedger;
using LedgerService.Domain.Constants;
using Messaging.Abstractions;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;

namespace LedgerService.Application.EventHandlers
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
                await CreateReversalEntryAsync(message, message.SourceAccountNumber, type);
                return;
            }

            await CreateReversalEntryAsync(message, message.SourceAccountNumber, TransactionType.Transfer);
            await CreateReversalEntryAsync(message, message.DestinationAccountNumber, TransactionType.Deposit);
        }

        private async Task CreateReversalEntryAsync(TransferFailedEvent message, string accountNumber, TransactionType type)
        {
            var direction = GetReversalDirection(message, accountNumber, type);

            _logger.LogInformation(
                "Creating reversal ledger entry for failed transaction {TransactionId}, account {AccountNumber}, amount {Amount}, currency {Currency}, type {Type}, direction {Direction}",
                message.TransactionId,
                accountNumber,
                message.Amount,
                message.Currency,
                type,
                direction
            );

            var createLedgerCommand = new CreateLedgerCommand
            {
                TransactionId = message.TransactionId,
                AccountNumber = accountNumber,
                Amount = message.Amount,
                Currency = message.Currency,
                Type = direction,
                Description = message.Description
            };

            await commandDispatcher.DispatchAsync<CreateLedgerCommand, ApiResponse<CreateLedgerCommandResult>>(createLedgerCommand);
        }

        private TransactionDirection GetReversalDirection(TransferFailedEvent message, string accountNumber, TransactionType type)
        {
            if (string.IsNullOrEmpty(message.DestinationAccountNumber))
            {
                if (type == TransactionType.Deposit)
                {
                    return TransactionDirection.Debit;
                }

                return TransactionDirection.Credit;
            }

            if (accountNumber == message.SourceAccountNumber)
            {
                return TransactionDirection.Credit;
            }

            return TransactionDirection.Debit;
        }
    }
}

