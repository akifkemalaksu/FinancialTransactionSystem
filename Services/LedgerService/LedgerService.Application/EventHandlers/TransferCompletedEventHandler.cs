using LedgerService.Application.Features.LedgerFeatures.CreateLedger;
using LedgerService.Domain.Constants;
using Messaging.Abstractions;
using Messaging.Contracts;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;

namespace LedgerService.Application.EventHandlers
{
    public class TransferCompletedEventHandler(
        ICommandDispatcher _commandDispatcher
    ) : IKafkaHandler<TransferCompletedEvent>
    {
        public async Task HandleAsync(TransferCompletedEvent message)
        {
            if (message.DestinationAccountNumber is null)
            {
                var transactionDirection = (TransactionType)message.Type == TransactionType.Withdraw
                    ? TransactionDirection.Debit
                    : TransactionDirection.Credit;

                await CreateLedgerEntryAsync(message, message.SourceAccountNumber, transactionDirection);
                return;
            }

            await CreateLedgerEntryAsync(message, message.SourceAccountNumber, TransactionDirection.Debit);
            await CreateLedgerEntryAsync(message, message.DestinationAccountNumber, TransactionDirection.Credit);
        }

        private async Task CreateLedgerEntryAsync(TransferCompletedEvent message, string accountNumber, TransactionDirection direction)
        {
            var createLedgerCommand = new CreateLedgerCommand
            {
                TransactionId = message.TransactionId,
                AccountNumber = accountNumber,
                Amount = message.Amount,
                Currency = message.Currency,
                Type = direction,
                Description = message.Description
            };
            await _commandDispatcher.DispatchAsync<CreateLedgerCommand, ApiResponse<CreateLedgerCommandResult>>(createLedgerCommand);
        }
    }
}
