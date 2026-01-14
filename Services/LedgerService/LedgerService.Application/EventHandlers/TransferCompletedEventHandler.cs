using LedgerService.Application.Features.LedgerFeatures.CreateLedger;
using LedgerService.Domain.Constants;
using Messaging.Abstractions;
using Messaging.Contracts;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Interfaces;

namespace LedgerService.Application.EventHandlers
{
    public class TransferCompletedEventHandler(
        ICommandDispatcher _commandDispatcher
    ) : IKafkaHandler<TransferCompletedEvent>
    {
        public async Task HandleAsync(TransferCompletedEvent message)
        {
            var direction = message.Amount >= 0 ? TransactionDirection.Credit : TransactionDirection.Debit;
            await CreateLedgerEntryAsync(message, message.AccountNumber, direction);
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
