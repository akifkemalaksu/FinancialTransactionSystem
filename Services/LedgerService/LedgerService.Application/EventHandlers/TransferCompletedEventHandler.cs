using LedgerService.Application.Features.LedgerFeatures.CreateLedger;
using LedgerService.Domain.Constants;
using Messaging.Abstractions;
using Messaging.Contracts;
using Microsoft.Extensions.Logging;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace LedgerService.Application.EventHandlers
{
    public class TransferCompletedEventHandler(
        ICommandDispatcher _commandDispatcher,
        ILogger<TransferCompletedEventHandler> _logger
    ) : IKafkaHandler<TransferCompletedEvent>
    {
        public async Task HandleAsync(TransferCompletedEvent message)
        {
            var direction = message.Amount >= 0 ? TransactionDirection.Credit : TransactionDirection.Debit;

            _logger.LogInformation(
                "Handling TransferCompletedEvent for transaction {TransactionId}, account {AccountNumber}, amount {Amount}, balance after {BalanceAfter}, direction {Direction}",
                message.TransactionId,
                message.AccountNumber,
                message.Amount,
                message.BalanceAfter,
                direction
            );

            await CreateLedgerEntryAsync(message, message.AccountNumber, direction);
        }

        private async Task CreateLedgerEntryAsync(TransferCompletedEvent message, string accountNumber, TransactionDirection direction)
        {
            var createLedgerCommand = new CreateLedgerCommand
            {
                TransactionId = message.TransactionId,
                AccountNumber = accountNumber,
                Amount = message.Amount,
                BalanceAfter = message.BalanceAfter,
                Currency = message.Currency,
                Type = direction,
                Description = message.Description
            };

            _logger.LogInformation(
                "Creating ledger entry for transaction {TransactionId}, account {AccountNumber}, amount {Amount}, direction {Direction}",
                message.TransactionId,
                accountNumber,
                message.Amount,
                direction
            );

            await _commandDispatcher.DispatchAsync<CreateLedgerCommand, ApiResponse<CreateLedgerCommandResult>>(createLedgerCommand);
        }
    }
}
