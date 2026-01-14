using LedgerService.Application.Features.LedgerFeatures.CreateLedger;
using LedgerService.Domain.Constants;
using Messaging.Abstractions;
using Messaging.Contracts;
using ServiceDefaults.Dtos.Responses;
using ServiceDefaults.Enums;
using ServiceDefaults.Interfaces;

namespace LedgerService.Application.EventHandlers
{
    public class TransferFailedEventHandler(
        ICommandDispatcher commandDispatcher
    ) : IKafkaHandler<TransferFailedEvent>
    {
        public async Task HandleAsync(TransferFailedEvent message)
        {
            var type = (TransactionType)message.Type;

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

